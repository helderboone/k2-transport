var Administrador = function () {
    //== Private Functions
    var initDataTable = function () {
        App.bloquear();

        $("#tblAdministrador").DataTable({
            ajax: {
                url: App.corrigirPathRota("listar-administradores"),
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
                               '<a href="#" data-id="' + row.id + '" class="alterar-administrador m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="left" title="" data-original-title="Alterar"><i class="la la-edit"></i></a>' +
                               '<a href="#" data-id="' + row.id + '" class="excluir-administrador m-portlet__nav-link btn m-btn m-btn--hover-danger m-btn--icon m-btn--icon-only m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="left" title="" data-original-title="Excluir"><i class="la la-trash"></i></a>';
                    }
                }
            ],
            select: {
                style: 'single',
                info: false
            },
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

            $("a[class*='alterar-administrador']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    manterAdministrador(id);
                });
            });

            $("a[class*='excluir-administrador']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    App.exibirConfirm("Deseja realmente excluir esse administrador?", "Sim", "Não", function () { excluirAdministrador(id); });
                });
            });
        }).on("processing.dt", function () {
            App.bloquear();
        });
    };

    var procurarAdministrador = function () {
        $("#frmProcurarAdministrador").validate({
            submitHandler: function () {
                $("#tblAdministrador").DataTable().ajax.reload();
            }
        });
    };

    var manterAdministrador = function (id) {
        var cadastro = id === null || id === 0;

        App.exibirModalPorRota((!cadastro ? App.corrigirPathRota("alterar-administrador/" + id) : App.corrigirPathRota("cadastrar-administrador")), function () {
            $("#iCpf, #iProcurarCpf").inputmask({
                "mask": "999.999.999-99"
            });

            $("#iCelular").inputmask({
                "mask": "(99) 99999-9999"
            });

            $("#frmManterAdministrador").validate({
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
                    }
                },

                submitHandler: function () {

                    var administrador = {
                        IdUsuario: $("#iIdAdministrador").val(),
                        Nome: $("#iNome").val(),
                        Email: $("#iEmail").val(),
                        Cpf: $("#iCpf").val(),
                        Rg: $("#iRg").val(),
                        Celular: $("#iCelular").val(),
                        Ativo: $("#cAtivo").is(':checked')
                    };

                    App.bloquear();

                    $.post(App.corrigirPathRota(cadastro ? "cadastrar-administrador" : "alterar-administrador"), { entrada: administrador })
                        .done(function (feedbackResult) {
                            var feedback = Feedback.converter(feedbackResult);

                            if (feedback.Tipo.Nome == Tipo.Sucesso) {
                                feedback.exibirModal(function ()
                                {
                                    $("#tblAdministrador").DataTable().ajax.reload();
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
                            App.desbloquear();
                        });
                }
            });
        });
    };

    var excluirAdministrador = function (id) {
        App.bloquear();

        $.post(App.corrigirPathRota("excluir-administrador/" + id), function (feedbackResult) {
            var feedback = Feedback.converter(feedbackResult);

            if (feedback.Tipo.Nome == Tipo.Sucesso) {
                feedback.exibirModal(function () {
                    $("#tblAdministrador").DataTable().ajax.reload();
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
                manterAdministrador(null);
            });

            $("#bProcurar").click(function () {
                procurarAdministrador();
            });
        }
    };
}();

jQuery(document).ready(function () {
    Administrador.init();
});