var Localidade = function () {
    //== Private Functions
    var initDataTable = function () {
        App.bloquear();

        $("#tblLocalidade").DataTable({
            ajax: {
                url: App.corrigirPathRota("listar-localidades"),
                type: "POST",
                error: function (jqXhr) {
                    var feedback = Feedback.converter(jqXhr.responseJSON);
                    feedback.exibirModal();
                },
                data: function (data) {
                    data.Nome = $("#iProcurarNome").val();
                    data.Uf = $("#sProcurarEstado").val();
                }
            },
            info: true,
            columns: [
                { data: "nome", title: "Nome", orderable: true, className: "all" },
                { data: "nomeUf", title: "Estado", orderable: true },
                {
                    data: null,
                    className: "td-actions dt-center",
                    orderable: false,
                    width: "70px",
                    render: function (data, type, row) {
                        return '<a href="#" data-id="' + row.id + '" class="alterar-localidade m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="left" title="" data-original-title="Alterar"><i class="la la-edit"></i></a>' +
                            '<a href="#" data-id="' + row.id + '" class="excluir-localidade m-portlet__nav-link btn m-btn m-btn--hover-danger m-btn--icon m-btn--icon-only m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="left" title="" data-original-title="Excluir"><i class="la la-trash"></i></a>';
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

            $("a[class*='alterar-localidade']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    manterLocalidade(id);
                });
            });

            $("a[class*='excluir-localidade']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    App.exibirConfirm("Deseja realmente excluir essa localidade?", "Sim", "Não", function () { excluirLocalidade(id); });
                });
            });
        }).on("processing.dt", function () {
            App.bloquear();
        });
    };

    var procurarLocalidade = function () {
        $("#frmProcurarLocalidade").validate({
            submitHandler: function () {
                $("#tblLocalidade").DataTable().ajax.reload();
            }
        });
    };

    var manterLocalidade = function (id) {
        var cadastro = id === null || id === 0;

        App.exibirModalPorRota((!cadastro ? App.corrigirPathRota("alterar-localidade/" + id) : App.corrigirPathRota("cadastrar-localidade")), function () {
            $("#sEstado").select2({
                placeholder: "Selecione um estado",
                dropdownParent: $('.jc-bs3-container')
            });

            $("#frmManterLocalidade").validate({
                rules: {
                    iNome: {
                        required: true
                    },
                    sEstado: {
                        required: true
                    }
                },

                submitHandler: function () {

                    var localidade = {
                        Id: $("#iIdLocalidade").val(),
                        Nome: $("#iNome").val(),
                        Uf: $("#sEstado").val()
                    };

                    App.bloquear($("#frmManterLocalidade"));

                    $.post(App.corrigirPathRota(cadastro ? "cadastrar-localidade" : "alterar-localidade"), { entrada: localidade })
                        .done(function (feedbackResult) {
                            var feedback = Feedback.converter(feedbackResult);

                            if (feedback.Tipo.Nome == Tipo.Sucesso) {
                                feedback.exibirModal(function () {
                                    $("#tblLocalidade").DataTable().ajax.reload();
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
                            App.desbloquear($("#frmManterLocalidade"));
                        });
                }
            });
        });
    };

    var excluirLocalidade = function (id) {
        App.bloquear();

        $.post(App.corrigirPathRota("excluir-localidade/" + id), function (feedbackResult) {
            var feedback = Feedback.converter(feedbackResult);

            if (feedback.Tipo.Nome == Tipo.Sucesso) {
                feedback.exibirModal(function () {
                    $("#tblLocalidade").DataTable().ajax.reload();
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

            $("#sProcurarEstado").select2({
                placeholder: "Selecione um estado",
                allowClear: true                
            });

            $("#bCadastrar").click(function () {
                manterLocalidade(null);
            });

            $("#bProcurar").click(function () {
                procurarLocalidade();
            });
        }
    };
}();

jQuery(document).ready(function () {
    Localidade.init();
});