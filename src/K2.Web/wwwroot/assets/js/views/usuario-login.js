//== Class Definition
var SnippetLogin = function() {

    var login = $('#m_login');

    //== Private Functions

    var displaySignInForm = function () {
        login.removeClass('m-login--forget-password');

        login.addClass('m-login--signin');
        mUtil.animateClass(login.find('.m-login__signin')[0], 'flipInX animated');
    };

    var displayForgetPasswordForm = function () {
        login.removeClass('m-login--signin');

        login.addClass('m-login--forget-password');
        mUtil.animateClass(login.find('.m-login__forget-password')[0], 'flipInX animated');

    };

    var handleFormSwitch = function () {
        $('#m_login_forget_password').click(function (e) {
            e.preventDefault();
            displayForgetPasswordForm();
        });

        $('#m_login_forget_password_cancel').click(function (e) {
            e.preventDefault();
            displaySignInForm();
        });
    };

    var handleSignInFormSubmit = function () {
        $('#m_login_signin_submit').click(function (e) {
            e.preventDefault();
            var btn = $(this);
            var form = $(this).closest('form');

            form.validate({
                rules: {
                    email: {
                        required: true,
                        email: true
                    },
                    password: {
                        required: true
                    }
                }
            });

            if (!form.valid()) {
                return;
            }

            btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

            form.ajaxSubmit({
                url: '',
                success: function () {
                    $.post(App.corrigirPathRota("login"), { email: $("#email").val(), senha: $("#password").val(), permanecerLogado: $("#remember").prop("checked") }, function (saida) {

                        if (saida != null) {
                            // Verifica se a saída é um "feedback"
                            if (saida.Tipo != null) {
                                var feedback = Feedback.converter(saida);

                                feedback.exibirModal();
                                return;
                            }

                            if (typeof (Storage) !== "undefined" && $("#remember").prop("checked")) {
                                localStorage.setItem("token", saida.token);
                            }

                            location.href = App.corrigirPathRota("viagens");
                        }
                    })
                    .fail(function (jqXhr) {
                        var feedback = Feedback.converter(jqXhr.responseJSON);
                        feedback.exibirModal();
                    })
                    .always(function () {
                        btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                    });
                }
            });
        });
    };

    var handleForgetPasswordFormSubmit = function () {
        $('#m_login_forget_password_submit').click(function (e) {
            e.preventDefault();

            var btn = $(this);
            var form = $(this).closest('form');

            form.validate({
                rules: {
                    emailRedefinir: {
                        required: true,
                        email: true
                    }
                }
            });

            if (!form.valid()) {
                return;
            }

            btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

            form.ajaxSubmit({
                url: '',
                success: function () {
                    $.post(App.corrigirPathRota("redefinir-senha/" + $("#emailRedefinir").val()), function (feedbackViewModel) {
                        var feedback = Feedback.converter(feedbackViewModel);

                        if (feedback.Tipo.Nome === Tipo.Sucesso)
                            displaySignInForm();
                        else
                            feedback.exibirModal();
                    })
                    .fail(function (jqXhr) {
                        var feedback = Feedback.converter(jqXhr.responseJSON);
                        feedback.exibirModal();
                    })
                    .always(function () {
                        btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                    });
                }
            });
        });
    };

    //== Public Functions
    return {
        // public functions
        init: function() {
            handleFormSwitch();
            handleSignInFormSubmit();
            handleForgetPasswordFormSubmit();

            if (typeof (Storage) !== "undefined") {
                var token = localStorage.getItem("token");

                if (token != "" && token != null) {
                    App.bloquear();

                    $.post(App.corrigirPathRota("login-por-token"), { token: token }, function (feedbackViewModel) {
                        var feedback = Feedback.converter(feedbackViewModel);

                        if (feedback.Tipo.Nome === Tipo.Sucesso)
                            location.href = App.corrigirPathRota("viagens");
                        else
                            console.info(feedback.Mensagem);
                    });
                }
            }
        }
    };
}();

//== Class Initialization
jQuery(document).ready(function() {
    SnippetLogin.init();
});