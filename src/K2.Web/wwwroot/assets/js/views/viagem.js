var Viagem = function () {
    //== Private Functions
    var obterViagensPrevistas = function () {
        $.get(App.corrigirPathRota("viagens-previstas"), function (html) {
            $(".viagens-previstas").html(html);

            mApp.initTooltips();

            $("#bCadastrar").click(function () {
                manterViagem(null);
            });

            $("a[class*='excluir-viagem']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    App.exibirConfirm("Deseja realmente excluir essa viagem?", "Sim", "Não", function () { excluirViagem(id, true); });
                });
            });

            $("a[class*='listar-reservas']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    obterReservasPorViagem(id);
                });
            });

        }).fail(function (jqXhr) {
            var feedback = Feedback.converter(jqXhr.responseJSON);
            feedback.exibirModal();
        });
    };

    var obterReservasPorViagem = function (idViagem) {
        App.exibirModalPorRota(App.corrigirPathRota("reservas-por-viagem/" + idViagem), function () {
            App.bloquear($("#portletReserva"));

            $("#tblReserva").DataTable({
                ajax: {
                    url: App.corrigirPathRota("listar-reservas-por-viagem/" + idViagem),
                    type: "POST",
                    error: function (jqXhr) {
                        var feedback = Feedback.converter(jqXhr.responseJSON);
                        feedback.exibirModal();
                    }
                },
                columns: [
                    {
                        data: null,
                        title: "Cliente",
                        orderable: false,
                        className: "all",
                        render: function (data, type, row) {
                            return '<span class="m--font-boldest">' + data.cliente.nome + '</span><br/>' +
                                'CPF: <span class="cpf">' + data.cliente.cpf + '</span><br/>' +
                                'RG: ' + data.cliente.rg + '<br/>' +
                                'Celular: <span class="celular">' + data.cliente.celular + '</span>';
                        }
                    },
                    {
                        data: null,
                        title: "Dependente",
                        className: "min-tablet",
                        orderable: false,
                        render: function (data, type, row) {
                            if (data.dependente === null)
                                return '';
                            else {
                                return '<span class="m--font-bold">' + data.dependente.nome + '</span><br/>' +
                                    'Data de nascimento: ' + data.dependente.dataNascimentoToString + '<br/>' +
                                    '<a href="#" data-id="' + row.id + '" class="alterar-reserva-dependente m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-container="#portletReserva" data-toggle="m-tooltip" data-placement="right" title="" data-original-title="Alterar dependente"><i class="la la-edit"></i></a>' +
                                    '<a href="#" data-id="' + row.id + '" class="excluir-reserva-dependente m-portlet__nav-link btn m-btn m-btn--hover-danger m-btn--icon m-btn--icon-only m-btn--pill" data-container="#portletReserva" data-toggle="m-tooltip" data-placement="right" title="" data-original-title="Excluir dependente"><i class="la la-trash"></i></a><br/>';
                            }
                        }
                    },
                    {
                        data: null,
                        title: "Valor pago",
                        orderable: false,
                        className: "dt-right min-tablet",
                        render: function (data, type, row) {
                            return accounting.formatMoney(data.valorPago, "R$ ", 2, ".", ",");
                        }
                    },
                    { data: "observacao", className: "min-tablet", title: "Observação", orderable: false },
                    {
                        data: null,
                        className: "td-actions dt-center all",
                        orderable: false,
                        width: "70px",
                        render: function (data, type, row) {
                            return '<span class="dropdown">' +
                                        '<a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="false"><i class="la la-ellipsis-h"></i></a>' + 
                                        '<div class="dropdown-menu dropdown-menu-right" x-placement="bottom-end" style="position: absolute; will-change: transform; top: 0px; left: 0px; transform: translate3d(-196px, 27px, 0px);">' +
                                (data.dependente !== null ? "" : '<a class="cadastrar-reserva-dependente dropdown-item" data-id="' + row.id + '" href="#"><i class="fa fa-plus"></i> Cadastrar dependente</a>') +
                                            '<a class="alterar-reserva dropdown-item" data-id="' + row.id + '" href="#"><i class="la la-edit"></i> Alterar reserva</a>' +
                                            '<a class="excluir-reserva dropdown-item" data-id="' + row.id + '" href="#"><i class="la la-trash"></i> Excluir reserva</a>' +
                                        '</div>' +
                                    '</span >';
                        }
                    }
                ],
                select: {
                    style: 'single',
                    info: false
                },
                autoWidth: false,
                info: false,
                serverSide: true,
                responsive: true,
                order: [0, "asc"],
                searching: false,
                paging: false,
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

                $("a[class*='cadastrar-reserva-dependente']").each(function () {
                    var idReserva = $(this).data("id");

                    $(this).click(function () {
                        manterReservaDependente(idReserva, true);
                    });
                });

                $("a[class*='alterar-reserva-dependente']").each(function () {
                    var idReserva = $(this).data("id");

                    $(this).click(function () {
                        manterReservaDependente(idReserva, false);
                    });
                });

                $("a[class*='excluir-reserva-dependente']").each(function () {
                    var idReserva = $(this).data("id");

                    $(this).click(function () {
                        App.exibirConfirm("Deseja realmente excluir o dependente dessa reserva?", "Sim", "Não", function () { excluirReservaDependente(idReserva); });
                    });
                });

                $("a[class*='alterar-reserva']").each(function () {
                    var id = $(this).data("id");

                    $(this).click(function () {
                        manterReserva(id);
                    });
                });
            }).on("processing.dt", function () {
                    App.desbloquear($("#portletReserva"));
            });
        }, true, "modal-reservas-por-viagem");
    };

    //var initDataTable = function () {
    //    App.bloquear();

    //    $("#tblViagem").DataTable({
    //        ajax: {
    //            url: App.corrigirPathRota("listar-viagens"),
    //            type: "POST",
    //            error: function (jqXhr) {
    //                var feedback = Feedback.converter(jqXhr.responseJSON);
    //                feedback.exibirModal();
    //            },
    //            data: function (data) {
    //                data.Nome = $("#iProcurarNome").val();
    //                data.Uf = $("#sProcurarEstado").val();
    //            }
    //        },
    //        info: true,
    //        columns: [
    //            { data: "nome", title: "Nome", orderable: true, className: "all" },
    //            { data: "uf", title: "Estado", orderable: true },
    //            {
    //                data: null,
    //                className: "td-actions dt-center",
    //                orderable: false,
    //                width: "70px",
    //                render: function (data, type, row) {
    //                    return '<a href="#" data-id="' + row.id + '" class="alterar-viagem m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="left" title="" data-original-title="Alterar"><i class="la la-edit"></i></a>' +
    //                        '<a href="#" data-id="' + row.id + '" class="excluir-viagem m-portlet__nav-link btn m-btn m-btn--hover-danger m-btn--icon m-btn--icon-only m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="left" title="" data-original-title="Excluir"><i class="la la-trash"></i></a>';
    //                }
    //            }
    //        ],
    //        select: {
    //            style: 'single',
    //            info: false
    //        },
    //        serverSide: true,
    //        responsive: true,
    //        pagingType: 'full_numbers',
    //        order: [0, "asc"],
    //        searching: false,
    //        paging: true,
    //        lengthChange: false,
    //        pageLength: 25
    //    }).on("draw.dt", function () {
    //        mApp.initTooltips();

    //        $("a[class*='alterar-viagem']").each(function () {
    //            var id = $(this).data("id");

    //            $(this).click(function () {
    //                manterViagem(id);
    //            });
    //        });

    //        $("a[class*='excluir-viagem']").each(function () {
    //            var id = $(this).data("id");

    //            $(this).click(function () {
    //                App.exibirConfirm("Deseja realmente excluir essa viagem?", "Sim", "Não", function () { excluirViagem(id); });
    //            });
    //        });
    //    }).on("processing.dt", function () {
    //        App.bloquear();
    //    });
    //};

    //var procurarViagem = function () {
    //    $("#frmProcurarViagem").validate({
    //        submitHandler: function () {
    //            $("#tblViagem").DataTable().ajax.reload();
    //        }
    //    });
    //};

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
                        LocaisEmbarque: $("#tLocaisEmbarque").val().split('\n'),
                        LocaisDesembarque: $("#tLocaisDesembarque").val().split('\n')
                    };

                    App.bloquear($("#frmManterViagem"));

                    $.post(App.corrigirPathRota(cadastro ? "cadastrar-viagem" : "alterar-viagem"), { entrada: viagem })
                        .done(function (feedbackResult) {
                            var feedback = Feedback.converter(feedbackResult);

                            if (feedback.Tipo.Nome === Tipo.Sucesso) {
                                feedback.exibirModal(function () {
                                    obterViagensPrevistas();
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

    var manterReserva = function (id) {
        var cadastro = id === null || id === 0;

        App.exibirModalPorRota((!cadastro ? App.corrigirPathRota("alterar-reserva/" + id) : App.corrigirPathRota("cadastrar-reserva")), function () {

            K2.criarSelectClientes("#sClienteReserva", true, "#portlet-manter-reserva");

            mApp.initTooltips();

            $('#iValorPago').inputmask('decimal', {
                radixPoint: ",",
                groupSeparator: ".",
                autoGroup: true,
                digits: 2,
                digitsOptional: false,
                placeholder: '0',
                rightAlign: false,
                prefix: ''
            });

            $("#frmManterReserva").validate({
                rules: {
                    sClienteReserva: {
                        required: true
                    }
                },

                submitHandler: function () {

                    //var viagem = {
                    //    Id: $("#iIdViagem").val(),
                    //    IdCarro: $("#sCarro").val(),
                    //    IdMotorista: $("#sMotorista").val(),
                    //    IdLocalidadeEmbarque: $("#sLocalidadeEmbarque").val(),
                    //    IdLocalidadeDesembarque: $("#sLocalidadeDesembarque").val(),
                    //    Descricao: $("#iDescricao").val(),
                    //    ValorPassagem: $("#iValorPassagem").val(),
                    //    DataHorarioSaida: $("#iDataHorarioSaida").val(),
                    //    LocaisEmbarque: $("#tLocaisEmbarque").val().split('\n'),
                    //    LocaisDesembarque: $("#tLocaisDesembarque").val().split('\n')
                    //};

                    //App.bloquear($("#frmManterViagem"));

                    //$.post(App.corrigirPathRota(cadastro ? "cadastrar-viagem" : "alterar-viagem"), { entrada: viagem })
                    //    .done(function (feedbackResult) {
                    //        var feedback = Feedback.converter(feedbackResult);

                    //        if (feedback.Tipo.Nome === Tipo.Sucesso) {
                    //            feedback.exibirModal(function () {
                    //                obterViagensPrevistas();
                    //                App.ocultarModal();
                    //            });
                    //        }
                    //        else
                    //            feedback.exibirModal();
                    //    })
                    //    .fail(function (jqXhr) {
                    //        var feedback = Feedback.converter(jqXhr.responseJSON);
                    //        feedback.exibirModal();
                    //    })
                    //    .always(function () {
                    //        App.desbloquear($("#frmManterViagem"));
                    //    });
                }
            });
        }, true, "manter-reserva");
    };

    var manterReservaDependente = function (idReserva, cadastro) {

        App.exibirModalPorRota((!cadastro ? App.corrigirPathRota("alterar-reserva-dependente/" + idReserva) : App.corrigirPathRota("cadastrar-reserva-dependente/" + idReserva)), function () {

            $("#iCpfDependente").inputmask({
                "mask": "999.999.999-99"
            });

            $("#iDataNascimentoDependente").inputmask("dd/mm/yyyy", {
                placeholder: "_"
            });

            $("#frmManterReservaDependente").validate({
                rules: {
                    iNomeDependente: {
                        required: true
                    },
                    iDataNascimentoDependente: {
                        required: true
                    }
                },

                submitHandler: function () {

                    var reservaDependente = {
                        IdReserva: $("#iIdReserva").val(),
                        Nome: $("#iNomeDependente").val(),
                        DataNascimento: $("#iDataNascimentoDependente").val(),
                        Cpf: $("#iCpfDependente").val(),
                        Rg: $("#iRgDependente").val()
                    };

                    App.bloquear($("#frmManterReservaDependente"));

                    $.post(App.corrigirPathRota(cadastro ? "cadastrar-reserva-dependente" : "alterar-reserva-dependente"), { entrada: reservaDependente })
                        .done(function (feedbackResult) {
                            var feedback = Feedback.converter(feedbackResult);

                            if (feedback.Tipo.Nome === Tipo.Sucesso) {
                                feedback.exibirModal(function () {
                                    $("#tblReserva").DataTable().ajax.reload();
                                    App.ocultarModalPorTitulo("manter-dependente");
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
                            App.desbloquear($("#frmManterReservaDependente"));
                        });
                }
            });

        }, true, "manter-dependente");
    };

    var excluirViagem = function (id, prevista) {
        App.bloquear();

        $.post(App.corrigirPathRota("excluir-viagem/" + id), function (feedbackResult) {
            var feedback = Feedback.converter(feedbackResult);

            if (feedback.Tipo.Nome === Tipo.Sucesso) {
                feedback.exibirModal(function () {
                    if (prevista)
                        obterViagensPrevistas();
                    else
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

    var excluirReservaDependente = function (idReserva) {
        App.bloquear($("#frmManterReservaDependente"));

        $.post(App.corrigirPathRota("excluir-reserva-dependente/" + idReserva), function (feedbackResult) {
            var feedback = Feedback.converter(feedbackResult);

            if (feedback.Tipo.Nome === Tipo.Sucesso) {
                feedback.exibirModal(function () {
                    $("#tblReserva").DataTable().ajax.reload();
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
            App.desbloquear($("#frmManterReservaDependente"));
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