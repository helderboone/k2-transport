var Cliente = function () {
    //== Private Functions
    var initDataTable = function () {
        App.bloquear();

        $("#tblCliente").DataTable({
            serverSide: true,
            ajax: {
                url: App.corrigirPathRota("listar-clientes"),
                type: "POST",
                error: function (xhr) {
                    //alert(xhr);
                    //App.exibirModalPorJqXHR(xhr);
                },
                data: function (data) {
                    data.Nome = "Jorge";//$("#txtNomeProcurar").val();
                    data.Email = "jlnpinheiro@gmail.com";//$("#txtEmailProcurar").val();
                }
            },
            info: true,
            columns: [
                //{
                //    data: "FlagAtivo",
                //    title: "Ativo",
                //    className: "center",
                //    orderable: true,
                //    width: "60px",
                //    render: function (data, type, row) {
                //        return "<div class=\"text-center\">" + (row.FlagAtivo === 1 ? "<span class=\"label label-success\">sim</span>" : "<span class=\"label label-danger\">não</span>") + "</div>";
                //    }
                //},
                { data: "Nome", title: "Nome", orderable: true },
                { data: "Email", title: "E-mail", orderable: true },
                { data: "Cpf", title: "CPF", orderable: false },
                { data: "Rg", title: "RG", orderable: false }
                //{
                //    data: null,
                //    className: "td-actions center all",
                //    orderable: false,
                //    width: "1px",
                //    render: function (data, type, row) {
                //        return "<button class=\"btn btn-default btn-round redefinir-senha-cliente\" data-id=\"" + row.IdUsuario + "\" data-toggle=\"popover\" data-trigger=\"hover\" data-placement=\"left\" title=\"Redefinir senha\" data-content=\"Redefinir a senha de acesso do cliente, por exemplo, em caso de esquecimento.\" data-container=\"body\"><i class=\"material-icons\">lock_open</i></button>";
                //    }
                //},
                //{
                //    data: null,
                //    className: "td-actions center all",
                //    orderable: false,
                //    width: "1px",
                //    render: function (data, type, row) {
                //        return "<button class=\"btn btn-primary btn-round alterar-cliente\" data-id=\"" + row.IdUsuario + "\" data-toggle=\"popover\" data-trigger=\"hover\" data-placement=\"left\" title=\"Alterar cliente\" data-content=\"Alterar as informações do cliente\" data-container=\"body\"><i class=\"material-icons\">edit</i></button>";
                //    }
                //},
                //{
                //    data: null,
                //    className: "td-actions center all",
                //    orderable: false,
                //    width: "1px",
                //    render: function (data, type, row) {
                //        return "<button class=\"btn btn-danger btn-round excluir-cliente\" data-id=\"" + row.IdUsuario + "\" data-toggle=\"popover\" data-trigger=\"hover\" data-placement=\"left\" title=\"Excluir cliente\" data-content=\"Exclui o cliente\" data-container=\"body\"><i class=\"material-icons\">delete</i></button>";
                //    }
                //}
            ],
            order: [1, "asc"],
            searching: false,
            paging: true,
            lengthChange: false,
            pageLength: 25
        }).on("draw.dt", function () {
            //InspecaoCondominial.initPopover();

            //$("button[class*='redefinir-senha-cliente']").each(function () {
            //    var id = $(this).data("id");

            //    $(this).click(function () {
            //        redefinirSenhaCliente(id);
            //    });
            //});

            //$("button[class*='alterar-cliente']").each(function () {
            //    var id = $(this).data("id");

            //    $(this).click(function () {
            //        manterCliente(id);
            //    });
            //});

            //$("button[class*='excluir-cliente']").each(function () {
            //    var id = $(this).data("id");

            //    $(this).click(function () {
            //        App.exibirModalConfirmacao("Deseja realmente excluir esse cliente?", "Atenção", "Sim", "Não", function () { excluirCliente(id); });
            //    });
            //});
        }).on("processing.dt", function () {
            App.bloquear();
        });
    };

    var manterCliente = function (id) {
        var cadastro = id === null || id === 0;

        App.exibirModalPorRota((!cadastro ? App.corrigirPathRota("alterar-cliente/" + id) : App.corrigirPathRota("cadastrar-cliente")), function () {
            $("#iCpf").inputmask("mm/dd/yyyy", {
                autoUnmask: true
            });

            $("#iCpf").inputmask({
                "mask": "999.999.999-99"
            });

            $("#iCelular").inputmask({
                "mask": "(99) 99999-9999"
            });

            $("#iCep").inputmask({
                "mask": "99.999-999"
            });

            $("#sEstado").select2({
                placeholder: "Selecione um estado",
                dropdownParent: $('.jc-bs3-container')
            });

            $("#frmManterCliente").validate({
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

                    var cliente = {
                        IdCliente: $("#iIdCliente").val(),
                        Nome: $("#iNome").val(),
                        Email: $("#iEmail").val(),
                        Cpf: $("#iCpf").val(),
                        Rg: $("#iRg").val(),
                        Celular: $("#iCelular").val(),
                        Ativo: $("#cAtivo").is(':checked'),
                        Cep: $("#iCep").val(),
                        Endereco: $("#iEndereco").val(),
                        Municipio: $("#iMunicipio").val(),
                        Uf: $("#sEstado").val()
                    };

                    App.bloquear($("#frmManterCliente"));

                    $.post(App.corrigirPathRota(cadastro ? "cadastrar-cliente" : "alterar-cliente"), { cadastrarClienteEntrada: cliente })
                        .done(function (feedbackResult) {
                            var feedback = Feedback.converter(feedbackResult);

                            if (feedback.Tipo.Nome == Tipo.Sucesso) {
                                feedback.exibirModal(function ()
                                {
                                    alert("Dar refresh aqui."); App.ocultarModal();
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
                            App.desbloquear($("#frmManterCliente"));
                        });
                }
            });
        });
    };

    //== Public Functions
    return {
        init: function () {
            initDataTable();

            $("#bCadastrar").click(function () {
                manterCliente(null);
            });
        }
    };
}();

jQuery(document).ready(function () {
    Cliente.init();
});