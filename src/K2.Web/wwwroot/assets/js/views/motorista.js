var Motorista = function () {
    //== Private Functions
    var initDataTable = function () {
        App.bloquear();

        $("#tblMotorista").DataTable({
            ajax: {
                url: App.corrigirPathRota("listar-motoristas"),
                type: "POST",
                error: function (jqXhr) {
                    var feedback = Feedback.converter(jqXhr.responseJSON);
                    feedback.exibirModal();
                },
                data: function (data) {
                    data.Nome = $("#iProcurarNome").val();
                    data.Email = $("#iProcurarEmail").val();
                    data.Cpf = $("#iProcurarCpf").val();
                    data.Rg = $("#iProcurarRg").val();
                    data.Cnh = $("#iProcurarCnh").val();
                }
            },
            info: true,
            columns: [
                { data: "nome", title: "Nome", orderable: true, className: "all" },
                {
                    data: "celular",
                    title: "Celular",
                    orderable: false,
                    createdCell: function (td, cellData, rowData, row, col) {
                        $(td).attr('class', 'celular');
                    }
                },
                { data: "email", title: "E-mail", orderable: true },
                {
                    data: "cpf",
                    title: "CPF",
                    orderable: false,
                    createdCell: function (td, cellData, rowData, row, col) {
                        $(td).attr('class', 'cpf');
                    }
                },
                { data: "rg", title: "RG", orderable: false },
                { data: "cnh", title: "CNH", orderable: false },
                {
                    data: "ativo",
                    title: "Ativo",
                    className: "dt-center",
                    orderable: false,
                    width: "30px",
                    render: function (data, type, row) {
                        return row.ativo ? '<span class="m-badge m-badge--success"></span>' : '<span class="m-badge m-badge--danger"></span>';
                    }
                },
                {
                    data: null,
                    className: "td-actions dt-center",
                    orderable: false,
                    width: "70px",
                    render: function (data, type, row) {
                        return '<a href="#" data-id-usuario="' + row.idUsuario + '" class="redefinir-senha m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="left" title="" data-original-title="Redefinir senha"><i class="fa fa-unlock"></i></a>' +
                               '<a href="#" data-id="' + row.id + '" class="alterar-motorista m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="left" title="" data-original-title="Alterar"><i class="la la-edit"></i></a>' +
                               '<a href="#" data-id="' + row.id + '" class="excluir-motorista m-portlet__nav-link btn m-btn m-btn--hover-danger m-btn--icon m-btn--icon-only m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="left" title="" data-original-title="Excluir"><i class="la la-trash"></i></a>';
                    }
                }
            ],
            serverSide: true,
            responsive: true,
			pagingType: 'full_numbers',
            order: [0, "asc"],
            searching: false,
            paging: true,
            lengthChange: false,
            pageLength: 25
        }).on("draw.dt", function () {
            mApp.initTooltips();

            $(".cpf").inputmask({
                "mask": "999.999.999-99"
            });

            $(".celular").inputmask({
                "mask": "(99) 99999-9999"
            });

            $("a[class*='redefinir-senha']").each(function () {
                var id = $(this).data("id-usuario");

                $(this).click(function () {
                    K2.redefinirSenha(id);
                });
            });

            $("a[class*='alterar-motorista']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    manterMotorista(id);
                });
            });

            $("a[class*='excluir-motorista']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    App.exibirConfirm("Deseja realmente excluir esse motorista?", "Sim", "Não", function () { excluirMotorista(id); });
                });
            });
        }).on("processing.dt", function () {
            App.bloquear();
        });
    };

    var procurarMotorista = function () {
        $("#frmProcurarMotorista").validate({
            submitHandler: function () {
                $("#tblMotorista").DataTable().ajax.reload();
            }
        });
    };

    var manterMotorista = function (id) {
        var cadastro = id === null || id === 0;

        App.exibirModalPorRota((!cadastro ? App.corrigirPathRota("alterar-motorista/" + id) : App.corrigirPathRota("cadastrar-motorista")), function () {
            $("#iCpf, #iProcurarCpf").inputmask({
                "mask": "999.999.999-99"
            });

            $("#iCelular").inputmask({
                "mask": "(99) 99999-9999"
            });

            $("#iCep").inputmask({
                "mask": "99.999-999"
            });

            $("#frmManterMotorista").validate({
                rules: {
                    iNome: {
                        required: true
                    },
                    iEmail: {
                        required: true,
                        email: true
                    },
                    iCpf: {
                        required: true
                    },
                    iRg: {
                        required: true
                    },
                    iCelular: {
                        required: true
                    },
                    iCnh: {
                        required: true
                    }
                },

                submitHandler: function () {

                    var motorista = {
                        Id: $("#iIdMotorista").val(),
                        Nome: $("#iNome").val(),
                        Email: $("#iEmail").val(),
                        Cpf: $("#iCpf").val(),
                        Rg: $("#iRg").val(),
                        Celular: $("#iCelular").val(),
                        Ativo: $("#cAtivo").is(':checked'),
                        Cnh: $("#iCnh").val()
                    };

                    App.bloquear($("#frmManterMotorista"));

                    $.post(App.corrigirPathRota(cadastro ? "cadastrar-motorista" : "alterar-motorista"), { entrada: motorista })
                        .done(function (feedbackResult) {
                            var feedback = Feedback.converter(feedbackResult);

                            if (feedback.Tipo.Nome == Tipo.Sucesso) {
                                feedback.exibirModal(function ()
                                {
                                    $("#tblMotorista").DataTable().ajax.reload();
                                    App.ocultarModal();
                                });
                            }
                            else
                                feedback.exibirModal();
                        })
                        .fail(function (jqXhr) {
                            var feedback = Feedback.converter(jqXhr.responseJSON);
                            feedback.exibirModal();
                        })
                        .always(function () {
                            App.desbloquear($("#frmManterMotorista"));
                        });
                }
            });
        });
    };

    var excluirMotorista = function (id) {
        App.bloquear();

        $.post(App.corrigirPathRota("excluir-motorista/" + id), function (feedbackResult) {
            var feedback = Feedback.converter(feedbackResult);

            if (feedback.Tipo.Nome == Tipo.Sucesso) {
                feedback.exibirModal(function () {
                    $("#tblMotorista").DataTable().ajax.reload();
                    App.ocultarModal();
                });
            }
            else
                feedback.exibirModal();
        })
        .fail(function (jqXhr) {
            var feedback = Feedback.converter(jqXhr.responseJSON);
            feedback.exibirModal();
        });
    };

    //== Public Functions
    return {
        init: function () {
            initDataTable();

            $("#bCadastrar").click(function () {
                manterMotorista(null);
            });

            $("#bProcurar").click(function () {
                procurarMotorista();
            });
        }
    };
}();

jQuery(document).ready(function () {
    Motorista.init();
});