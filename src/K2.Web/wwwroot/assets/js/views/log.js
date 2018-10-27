var Log = function () {
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
                    className: "all m--align-center",
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
                    className: "td-actions dt-center",
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
            if ($("#divAceEditor").length)
            {
                var editor = ace.edit("divAceEditor");
                editor.setTheme("ace/theme/chrome");
                editor.getSession().setMode("ace/mode/json");
                editor.setReadOnly(true);
            }
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
        }
    };
}();

jQuery(document).ready(function () {
    Log.init();
});