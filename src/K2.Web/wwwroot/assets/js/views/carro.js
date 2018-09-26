var Carro = function () {
    //== Private Functions
    var initDataTable = function () {
        App.bloquear();

        $("#tblCarro").DataTable({
            ajax: {
                url: App.corrigirPathRota("listar-carros"),
                type: "POST",
                error: function (jqXhr) {
                    var feedback = Feedback.converter(jqXhr.responseJSON);
                    feedback.exibirModal();
                },
                data: function (data) {
                    data.Descricao = $("#iProcurarDescricao").val();
                    data.NomeFabricante = $("#iProcurarNomeFabricante").val();
                    data.AnoModelo = $("#iProcurarAnoModelo").val();
                    data.Placa = $("#iProcurarPlaca").val();
                    data.Renavam = $("#iProcurarRenavam").val();
                }
            },
            info: true,
            columns: [
                { data: "descricao", title: "Descrição", orderable: true, className: "all" },
                {
                    data: null,
                    title: "Proprietário",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.proprietario.nome;
                    }
                },
                { data: "nomeFabricante", title: "Fabricante", orderable: true },
                { data: "anoModelo", title: "Ano / modelo", orderable: true },
                { data: "quantidadeLugares", title: "Qtd. lugares", orderable: true },
                { data: "placa", title: "Placa", orderable: true },
                { data: "renavam", title: "Renavam", orderable: false },
                {
                    data: null,
                    className: "td-actions dt-center",
                    orderable: false,
                    width: "70px",
                    render: function (data, type, row) {
                        return '<a href="#" data-id="' + row.id + '" class="alterar-carro m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="left" title="" data-original-title="Alterar"><i class="la la-edit"></i></a>' +
                            '<a href="#" data-id="' + row.id + '" class="excluir-carro m-portlet__nav-link btn m-btn m-btn--hover-danger m-btn--icon m-btn--icon-only m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="left" title="" data-original-title="Excluir"><i class="la la-trash"></i></a>';
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

            $("a[class*='alterar-carro']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    manterCarro(id);
                });
            });

            $("a[class*='excluir-carro']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    App.exibirConfirm("Deseja realmente excluir esse carro?", "Sim", "Não", function () { excluirCarro(id); });
                });
            });
        }).on("processing.dt", function () {
            App.bloquear();
        });
    };

    var procurarCarro = function () {
        $("#frmProcurarCarro").validate({
            submitHandler: function () {
                $("#tblCarro").DataTable().ajax.reload();
            }
        });
    };

    var manterCarro = function (id) {
        var cadastro = id === null || id === 0;

        App.exibirModalPorRota((!cadastro ? App.corrigirPathRota("alterar-carro/" + id) : App.corrigirPathRota("cadastrar-carro")), function () {
            //$("#sEstado").select2({
            //    placeholder: "Selecione um estado",
            //    dropdownParent: $('.jc-bs3-container')
            //});

            $("#frmManterCarro").validate({
                rules: {
                    iNome: {
                        required: true
                    },
                    sEstado: {
                        required: true
                    }
                },

                submitHandler: function () {

                    var carro = {
                        Id: $("#iIdCarro").val(),
                        Nome: $("#iNome").val(),
                        Uf: $("#sEstado").val()
                    };

                    App.bloquear($("#frmManterCarro"));

                    $.post(App.corrigirPathRota(cadastro ? "cadastrar-carro" : "alterar-carro"), { entrada: carro })
                        .done(function (feedbackResult) {
                            var feedback = Feedback.converter(feedbackResult);

                            if (feedback.Tipo.Nome == Tipo.Sucesso) {
                                feedback.exibirModal(function () {
                                    $("#tblCarro").DataTable().ajax.reload();
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
                            App.desbloquear($("#frmManterCarro"));
                        });
                }
            });
        });
    };

    var excluirCarro = function (id) {
        App.bloquear();

        $.post(App.corrigirPathRota("excluir-carro/" + id), function (feedbackResult) {
            var feedback = Feedback.converter(feedbackResult);

            if (feedback.Tipo.Nome == Tipo.Sucesso) {
                feedback.exibirModal(function () {
                    $("#tblCarro").DataTable().ajax.reload();
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

            $("#iPlaca, #iProcurarPlaca").inputmask({
                "mask": "aaa-9999"
            });

            $("#bCadastrar").click(function () {
                manterCarro(null);
            });

            $("#bProcurar").click(function () {
                procurarCarro();
            });
        }
    };
}();

jQuery(document).ready(function () {
    Carro.init();
});