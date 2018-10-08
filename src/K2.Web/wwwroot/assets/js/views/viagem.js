var Viagem = function () {
    //== Private Functions
    var obterViagensPrevistas = function () {
        $.get(App.corrigirPathRota("viagens-previstas"), function (html) {
            $(".viagens-previstas").html(html);

            $("#bCadastrar").click(function () {
                manterViagem(null);
            });

            mApp.initTooltips();
        }).fail(function (jqXhr) {
            var feedback = Feedback.converter(jqXhr.responseJSON);
            feedback.exibirModal();
        });
    };

    var initDataTable = function () {
        App.bloquear();

        $("#tblViagem").DataTable({
            ajax: {
                url: App.corrigirPathRota("listar-viagens"),
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
                { data: "uf", title: "Estado", orderable: true },
                {
                    data: null,
                    className: "td-actions dt-center",
                    orderable: false,
                    width: "70px",
                    render: function (data, type, row) {
                        return '<a href="#" data-id="' + row.id + '" class="alterar-viagem m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="left" title="" data-original-title="Alterar"><i class="la la-edit"></i></a>' +
                            '<a href="#" data-id="' + row.id + '" class="excluir-viagem m-portlet__nav-link btn m-btn m-btn--hover-danger m-btn--icon m-btn--icon-only m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="left" title="" data-original-title="Excluir"><i class="la la-trash"></i></a>';
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

            $("a[class*='alterar-viagem']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    manterViagem(id);
                });
            });

            $("a[class*='excluir-viagem']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    App.exibirConfirm("Deseja realmente excluir essa viagem?", "Sim", "Não", function () { excluirViagem(id); });
                });
            });
        }).on("processing.dt", function () {
            App.bloquear();
        });
    };

    var procurarViagem = function () {
        $("#frmProcurarViagem").validate({
            submitHandler: function () {
                $("#tblViagem").DataTable().ajax.reload();
            }
        });
    };

    var manterViagem = function (id) {
        var cadastro = id === null || id === 0;

        App.exibirModalPorRota((!cadastro ? App.corrigirPathRota("alterar-viagem/" + id) : App.corrigirPathRota("cadastrar-viagem")), function () {
            $("#sMotorista").select2({
                placeholder: "Selecione um motorista",
                dropdownParent: $('.jc-bs3-container')
            });

            $("#sCarro").select2({
                placeholder: "Selecione um carro",
                dropdownParent: $('.jc-bs3-container')
            });

            $("#sLocalidadeEmbarque").select2({
                placeholder: "Selecione a localidade de embarque",
                dropdownParent: $('.jc-bs3-container')
            });

            $("#sLocalidadeDesembarque").select2({
                placeholder: "Selecione a localidade de desembarque",
                dropdownParent: $('.jc-bs3-container')
            });

            var startDate = $("#iDataHorarioSaida").data("startdate");

            $('#iDataHorarioSaida').datetimepicker({
                todayHighlight: false,
                autoclose: true,
                pickerPosition: 'bottom-left',
                format: 'dd/mm/yyyy hh:ii',
                startDate: startDate,
                language: 'pt-BR'
            });

            $('#iValorPassagem').inputmask('decimal', {
                radixPoint: ",",
                groupSeparator: ".",
                autoGroup: true,
                digits: 2,
                digitsOptional: false,
                placeholder: '0',
                rightAlign: false,
                prefix: '',
                onBeforeMask: function (value, opts) {
                    return value;
                }
            });

            $("#frmManterViagem").validate({
                rules: {
                    iDescricao: {
                        required: true
                    },
                    iDataHorarioSaida: {
                        required: true
                    },
                    sMotorista: {
                        required: true
                    },
                    sCarro: {
                        required: true
                    },
                    sLocalidadeEmbarque: {
                        required: true
                    },
                    sLocalidadeDesembarque: {
                        required: true
                    }
                },

                submitHandler: function () {

                    var viagem = {
                        Id: $("#iIdViagem").val(),
                        IdCarro: $("#sCarro").val(),
                        IdMotorista: $("#sMotorista").val(),
                        IdLocalidadeEmbarque: $("#sLocalidadeEmbarque").val(),
                        IdLocalidadeDesembarque: $("#sLocalidadeDesembarque").val(),
                        Descricao: $("#iDescricao").val(),
                        ValorPassagem: $("#iValorPassagem").val(),
                        DataHorarioSaida: $("#iDataHorarioSaida").val(),
                        LocaisEmbarque: "",
                        LocaisDesembarque: ""
                    };

                    App.bloquear($("#frmManterViagem"));

                    $.post(App.corrigirPathRota(cadastro ? "cadastrar-viagem" : "alterar-viagem"), { entrada: viagem })
                        .done(function (feedbackResult) {
                            var feedback = Feedback.converter(feedbackResult);

                            if (feedback.Tipo.Nome == Tipo.Sucesso) {
                                feedback.exibirModal(function () {
                                    $("#tblViagem").DataTable().ajax.reload();
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
                            App.desbloquear($("#frmManterViagem"));
                        });
                }
            });
        });
    };

    var excluirViagem = function (id) {
        App.bloquear();

        $.post(App.corrigirPathRota("excluir-viagem/" + id), function (feedbackResult) {
            var feedback = Feedback.converter(feedbackResult);

            if (feedback.Tipo.Nome == Tipo.Sucesso) {
                feedback.exibirModal(function () {
                    $("#tblViagem").DataTable().ajax.reload();
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
            obterViagensPrevistas();
        }
    };
}();

jQuery(document).ready(function () {
    Viagem.init();
});