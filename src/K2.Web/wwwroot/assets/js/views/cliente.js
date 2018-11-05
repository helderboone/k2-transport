var Cliente = function () {
    //== Private Functions
    var initDataTable = function () {
        App.bloquear();

        $("#tblCliente").DataTable({
            ajax: {
                url: App.corrigirPathRota("listar-clientes"),
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
                               '<a href="#" data-id="' + row.id + '" class="alterar-cliente m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="left" title="" data-original-title="Alterar"><i class="la la-edit"></i></a>' +
                               '<a href="#" data-id="' + row.id + '" class="excluir-cliente m-portlet__nav-link btn m-btn m-btn--hover-danger m-btn--icon m-btn--icon-only m-btn--pill" data-container="body" data-toggle="m-tooltip" data-placement="left" title="" data-original-title="Excluir"><i class="la la-trash"></i></a>';
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

            $("a[class*='alterar-cliente']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    K2.manterCliente(id, function () { $("#tblCliente").DataTable().ajax.reload(); });
                });
            });

            $("a[class*='excluir-cliente']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    App.exibirConfirm("Deseja realmente excluir esse cliente?", "Sim", "Não", function () { excluirCliente(id); });
                });
            });
        }).on('responsive-display', function (e, datatable, row, showHide, update) {
            $("a[class*='redefinir-senha']").each(function () {
                var id = $(this).data("id-usuario");

                $(this).click(function () {
                    K2.redefinirSenha(id);
                });
            });

            $("a[class*='alterar-cliente']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    K2.manterCliente(id, function () { $("#tblCliente").DataTable().ajax.reload(); });
                });
            });

            $("a[class*='excluir-cliente']").each(function () {
                var id = $(this).data("id");

                $(this).click(function () {
                    App.exibirConfirm("Deseja realmente excluir esse cliente?", "Sim", "Não", function () { excluirCliente(id); });
                });
            });
        }).on("processing.dt", function () {
            App.bloquear();
        });
    };

    var procurarCliente = function () {
        $("#frmProcurarCliente").validate({
            submitHandler: function () {
                $("#tblCliente").DataTable().ajax.reload();
            }
        });
    };

    var excluirCliente = function (id) {
        App.bloquear();

        $.post(App.corrigirPathRota("excluir-cliente/" + id), function (feedbackResult) {
            var feedback = Feedback.converter(feedbackResult);

            if (feedback.Tipo.Nome == Tipo.Sucesso) {
                feedback.exibirModal(function () {
                    $("#tblCliente").DataTable().ajax.reload();
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
                K2.manterCliente(null, function () { $("#tblCliente").DataTable().ajax.reload(); });
            });

            $("#bProcurar").click(function () {
                procurarCliente();
            });
        }
    };
}();

jQuery(document).ready(function () {
    Cliente.init();
});