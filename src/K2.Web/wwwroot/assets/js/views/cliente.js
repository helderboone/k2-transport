var Cliente = function () {
    //== Private Functions
    var manterCliente = function (id) {
        var cadastro = (id == null || id === 0);

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
            $("#bCadastrar").click(function () {
                manterCliente();
            });
        }
    };
}();

jQuery(document).ready(function () {
    Cliente.init();
});