﻿var ProprietarioCarro = function () {
    //== Private Functions
    var initDataTable = function () {
        App.bloquear();

        $("#tblProprietarioCarro").DataTable({
            ajax: {
                url: App.corrigirPathRota("listar-proprietarios"),
                type: "POST",
                error: function (jqXhr) {
                    var feedback = Feedback.converter(jqXhr.responseJSON);
                    feedback.exibirModal();
                },
                data: function (data) {
                    data.Nome  = $("#iProcurarNome").val();
                    data.Email = $("#iProcurarEmail").val();
                    data.Cpf   = $("#iProcurarCpf").val();
                    data.Rg    = $("#iProcurarRg").val();
                }
            },
            info: true,
            columns: [
                { data: "nome", title: "Nome", orderable: true, className: "all" },
                {
                    data: null,
                    title: "Carros",
                    orderable: false,
                    render: function (data, type, row) {
                        if (row.carros === null || row.carros.length === 0)
                            return "<i>Nenhum carro relacionado.</i>";

                        var html = "";

                        for (var i = 0; i < row.carros.length; i++) {
                            html += "- " + row.carros[i].descricao + ' <a href="#" data-id="' + row.carros[i].id + '" class="visualizar-carro btn m-btn m-btn--hover-info m-btn--icon m-btn--icon-only--sm m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="right" title="" data-original-title="Informações"><i class="fa fa-info"></i></a><br/>';
                        }

                        return html;
                    }
                },
                { data: "celularFormatado", title: "Celular", orderable: false },
                { data: "email", title: "E-mail", orderable: true },
                { data: "cpfFormatado", title: "CPF", orderable: false },
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
                            '<a href="#" data-id="' + row.id + '" class="alterar-motorista m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="left" title="" data-original-title="Alterar"><i class="la la-edit"></i></a>' +
                            '<a href="#" data-id="' + row.id + '" class="excluir-motorista m-portlet__nav-link btn m-btn m-btn--hover-danger m-btn--icon m-btn--icon-only m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="left" title="" data-original-title="Excluir"><i class="la la-trash"></i></a>';
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

            $("a[class*='redefinir-senha']").each(function () {
                var id = $(this).data("id-usuario");

                $(this).click(function () {
                    K2.redefinirSenha(id);
                });
            });

            $("a[class*='visualizar-carro']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    K2.visualizarCarro(id);
                });
            });

            $("a[class*='alterar-motorista']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    manterProprietarioCarro(id);
                });
            });

            $("a[class*='excluir-motorista']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    App.exibirConfirm("Deseja realmente excluir esse proprietário?", "Sim", "Não", function () { excluirProprietarioCarro(id); });
                });
            });
        }).on("processing.dt", function () {
            App.bloquear();
        });
    };

    var procurarProprietarioCarro = function () {
        $("#frmProcurarProprietarioCarro").validate({
            submitHandler: function () {
                $("#tblProprietarioCarro").DataTable().ajax.reload();
            }
        });
    };

    var manterProprietarioCarro = function (id) {
        var cadastro = id === null || id === 0;

        App.exibirModalPorRota((!cadastro ? App.corrigirPathRota("alterar-proprietario/" + id) : App.corrigirPathRota("cadastrar-proprietario")), function () {
            $("#iCpf, #iProcurarCpf").inputmask({
                mask: "999.999.999-99",
                clearIncomplete: true
            });

            $("#iCelular").inputmask({
                mask: "(99) 99999-9999",
                clearIncomplete: true
            });

            $("#iCep").inputmask({
                mask: "99.999-999",
                clearIncomplete: true
            });

            $("#frmManterProprietarioCarro").validate({
                rules: {
                    iNome: {
                        required: true
                    },
                    iCelular: {
                        required: true
                    }
                },

                submitHandler: function () {

                    var proprietario = {
                        Id: $("#iIdProprietarioCarro").val(),
                        Nome: $("#iNome").val(),
                        Email: $("#iEmail").val(),
                        Cpf: $("#iCpf").val(),
                        Rg: $("#iRg").val(),
                        Celular: $("#iCelular").val(),
                        Ativo: $("#cAtivo").is(':checked')
                    };

                    App.bloquear();

                    $.post(App.corrigirPathRota(cadastro ? "cadastrar-proprietario" : "alterar-proprietario"), { entrada: proprietario })
                        .done(function (feedbackResult) {
                            var feedback = Feedback.converter(feedbackResult);

                            if (feedback.Tipo.Nome == Tipo.Sucesso) {
                                feedback.exibirModal(function () {
                                    $("#tblProprietarioCarro").DataTable().ajax.reload();
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

    var excluirProprietarioCarro = function (id) {
        App.bloquear();

        $.post(App.corrigirPathRota("excluir-proprietario/" + id), function (feedbackResult) {
            var feedback = Feedback.converter(feedbackResult);

            if (feedback.Tipo.Nome == Tipo.Sucesso) {
                feedback.exibirModal(function () {
                    $("#tblProprietarioCarro").DataTable().ajax.reload();
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
                manterProprietarioCarro(null);
            });

            $("#bProcurar").click(function () {
                procurarProprietarioCarro();
            });
        }
    };
}();

jQuery(document).ready(function () {
    ProprietarioCarro.init();
});