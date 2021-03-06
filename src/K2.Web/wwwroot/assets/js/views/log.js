﻿var Log = function () {
    //== Private Functions
    var initDataTable = function () {
        App.bloquear();

        $("#tblLog").DataTable({
            ajax: {
                url: App.corrigirPathRota("listar-log"),
                type: "POST",
                error: function (jqXhr) {
                    var feedback = Feedback.converter(jqXhr.responseJSON);
                    feedback.exibirModal();
                },
                data: function (data) {
                    data.DataInicio = $("#iProcurarDataSaidaInicio").val();
                    data.DataFim = $("#iProcurarDataSaidaFim").val();
                    data.Tipo = $("#sProcurarTipo").val();
                    data.Mensagem = $("#iProcurarMensagem").val();
                    data.Usuario = $("#iProcurarUsuario").val();
                    data.NomeOrigem = $("#iProcurarOrigem").val();
                    data.ExceptionInfo = $("#iProcurarExceptionInfo").val();
                }
            },
            info: true,
            columns: [
                { data: "dataToString", title: "Data", width: "1px", orderable: true, className: "all" },
                {
                    data: null,
                    className: "m--align-center",
                    orderable: true,
                    width: "1px",
                    createdCell: function (td, cellData, rowData, row, col) {
                        switch (cellData.tipo) {
                            case "Information":
                                $(td).addClass('bg-info m--font-light m--font-bolder');
                                $(td).html("Info");
                                break;
                            case "Warning":
                                $(td).addClass('bg-warning m--font-bolder');
                                $(td).html("Atenção");
                                break;
                            case "Error":
                                $(td).addClass('bg-danger m--font-light m--font-bolder');
                                $(td).html("Erro");
                                break;
                        }

                        
                    }
                },
                { data: "mensagem", title: "Mensagem", orderable: true },
                { data: "usuario", title: "Usuário", orderable: true },
                { data: "nomeOrigem", title: "Origem", orderable: true },
                {
                    data: null,
                    className: "td-actions dt-center all",
                    orderable: false,
                    width: "70px",
                    render: function (data, type, row) {
                        return '<a href="#" data-id="' + row.id + '" class="detalhar-log m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="left" title="" data-original-title="Detalhar"><i class="la la-search"></i></a>' +
                            '<a href="#" data-id="' + row.id + '" class="excluir-log m-portlet__nav-link btn m-btn m-btn--hover-danger m-btn--icon m-btn--icon-only m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="left" title="" data-original-title="Excluir"><i class="la la-trash"></i></a>';
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
            order: [0, "desc"],
            searching: false,
            paging: true,
            lengthChange: false,
            pageLength: 15
        }).on("draw.dt", function () {
            mApp.initTooltips();

            $("a[class*='detalhar-log']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    detalharLog(id);
                });
            });

            $("a[class*='excluir-log']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    App.exibirConfirm("Deseja realmente excluir esse registro?", "Sim", "Não", function () { excluirLog(id); });
                });
            });
        }).on("processing.dt", function () {
            App.bloquear();
        });
    };

    var procurarLog = function () {
        $("#frmProcurarLog").validate({
            submitHandler: function () {
                $("#tblLog").DataTable().ajax.reload();
            }
        });
    };

    var detalharLog = function (id) {
        App.exibirModalPorRota(App.corrigirPathRota("detalhar-log/" + id), function () {
            mApp.initComponents();
        });
    };

    var excluirLog = function (id) {
        App.bloquear();

        $.post(App.corrigirPathRota("excluir-log/" + id), function (feedbackResult) {
            var feedback = Feedback.converter(feedbackResult);

            if (feedback.Tipo.Nome == Tipo.Sucesso) {
                feedback.exibirModal(function () {
                    $("#tblLog").DataTable().ajax.reload();
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

    var tokenSlack = null;
    var channelIdSlack = null;
    var mensagensSlack = [];

    var totalMensagens = 0;
    var contExcluido = 0;
    var porcentagem = 0;

    var limparLogSlack = function () {
        
        let historyUrl = 'https://slack.com/api/groups.history?token=' + tokenSlack + '&count=1000&channel=' + channelIdSlack;

        $.get(historyUrl, function (res) {
            try {
                if (res == null) {
                    let feedback = new Feedback(3, "Ocorreu um erro ao excluir os registros do Slack.", "A API do Slack não retornou uma resposta.");
                    feedback.exibirModal();
                    return;
                }

                if (!res.ok) {
                    let feedback = new Feedback(3, "Ocorreu um erro ao excluir os registros do Slack.", "A API do Slack retornou o seguinte código de erro: <b>" + res.error + "</b>");
                    feedback.exibirModal();
                    return;
                }
                else {
                    if (res.messages.length == 0) {
                        let feedback = new Feedback(2, "Nenhuma mensagem foi encontrada.");
                        feedback.exibirModal();
                        return;
                    }

                    for (var i = 0; i < res.messages.length; i++) {
                        mensagensSlack.push(res.messages[i].ts);
                    }

                    totalMensagens = mensagensSlack.length;

                    App.desbloquear();
                    $("#labelProgress").html("Serão excluídas, " + mensagensSlack.length + " mensagens.");

                    $('#modal_Progress').modal({ show: true, backdrop: 'static', keyboard: false })
                        .on('shown.bs.modal', function (e) {
                            contExcluido = 0;
                            excluirLogSlack();
                        });
                }
            } catch (e) {
                let feedback = new Feedback(3, "Ocorreu um erro ao excluir os registros do Slack.", e);
                feedback.exibirModal();
                App.desbloquear();
            }
        });
    };

    var excluirLogSlack = function () {

        if (mensagensSlack.length == 0) {
            let feedback = new Feedback(4, "Todas as mensagens foram excluídas com sucesso.");
            feedback.exibirModal(function () {
                $('#modal_Progress').modal('hide');
            });
            return;
        }

        let ts = mensagensSlack.shift();

        let deleteUrl = 'https://slack.com/api/chat.delete?token=' + tokenSlack + '&channel=' + channelIdSlack + '&ts=' + ts;

        $.get(deleteUrl, function (res) {
            if (res != null && res.ok) {
                contExcluido++;

                porcentagem = parseInt(contExcluido * 100 / totalMensagens);

                $("#labelProgress").html("Excluindo mensagem " + contExcluido + " de " + totalMensagens + "...");

                $(".progress-bar").width(porcentagem + "%");
            }
            else {
                if (res != null && !res.ok) {
                    let feedback = new Feedback(3, "Ocorreu um erro ao excluir os registros do Slack.", "A API do Slack retornou o seguinte código de erro: <b>" + res.error + "</b>");
                    feedback.exibirModal();
                    return;
                }

                mensagensSlack.push(ts);
                contExcluido--;
            }
        });

        setTimeout(excluirLogSlack, 1000);
    }

    //== Public Functions
    return {
        init: function () {
            initDataTable();

            $("#sProcurarTipo").select2({
                placeholder: "Selecione um tipo",
                allowClear: true
            });

            $('#iProcurarPeriodoSaida').daterangepicker({
                buttonClasses: 'm-btn btn',
                applyClass: 'btn btn-sm m-btn--square btn-info',
                cancelClass: 'btn btn-sm m-btn--square btn-secondary',
                parentEl: '#modal_Procurar',
                minDate: moment().startOf('year'),
                maxDate: moment().subtract(1, 'days'),
                locale: {
                    format: "DD/MM/YYYY",
                    applyLabel: "Aplicar",
                    cancelLabel: "Cancelar",
                    daysOfWeek: [
                        "Dom",
                        "Seg",
                        "Ter",
                        "Qua",
                        "Qui",
                        "Sex",
                        "Sáb"
                    ],
                    monthNames: [
                        "Janeiro",
                        "Fevereiro",
                        "Março",
                        "Abril",
                        "Maio",
                        "Junho",
                        "Julho",
                        "Agosto",
                        "Setembro",
                        "Outubro",
                        "Novembro",
                        "Dezembro"
                    ]
                }
            }, function (start, end, label) {
                $('#iProcurarPeriodoSaida .form-control').val(start.format('DD/MM/YYYY') + ' até ' + end.format('DD/MM/YYYY'));
                $("#iProcurarDataSaidaInicio").val(start.format('DD/MM/YYYY'));
                $("#iProcurarDataSaidaFim").val(end.format('DD/MM/YYYY'));
            });

            $('#iProcurarPeriodoSaida').on('cancel.daterangepicker', function (ev, picker) {
                $('#iProcurarPeriodoSaida .form-control').val('');
                $("#iProcurarDataSaidaInicio").val('');
                $("#iProcurarDataSaidaFim").val('');
            });

            $("#bProcurar").click(function () {
                procurarLog();
            });

            $("#bLimparLog").click(function () {
                App.exibirConfirm("Deseja realmente limpar todos os registros do log no banco de dados?", "Sim", "Não", function () {
                    $.post(App.corrigirPathRota("limpar-log"), function (feedbackResult) {
                        var feedback = Feedback.converter(feedbackResult);

                        if (feedback.Tipo.Nome == Tipo.Sucesso) {
                            feedback.exibirModal(function () {
                                $("#tblLog").DataTable().ajax.reload();
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
                });
            });

            $("#bLimparSlack").click(function () {
                tokenSlack = $(this).data("token");
                channelIdSlack = $(this).data("channel-id");

                App.exibirConfirm("Deseja realmente limpar todos os registros do canal no Slack?", "Sim", "Não", function ()
                {
                    limparLogSlack();
                });
            });
        }
    };
}();

jQuery(document).ready(function () {
    Log.init();
});