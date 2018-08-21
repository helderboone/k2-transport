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
                    $.post(App.corrigirPathRota("autenticar"), { email: $("#email").val(), senha: $("#password").val(), permanecerLogado: $("#remember").prop("checked") }, function (feedbackViewModel) {
                        var feedback = Feedback.converter(feedbackViewModel);

                        if (feedback.Tipo.Nome == Tipo.Sucesso)
                            location.href = App.corrigirPathRota("inicio");
                        else
                            Feedback.exibirModal(feedbackViewModel);
                    })
                    .fail(function (feedbackViewModel) {
                        Feedback.exibirModal(feedbackViewModel);
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
                    email: {
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
                    // similate 2s delay
                    setTimeout(function () {
                        btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false); // remove 
                        form.clearForm(); // clear form
                        form.validate().resetForm(); // reset validation states

                        // display signup form
                        displaySignInForm();
                        var signInForm = login.find('.m-login__signin form');
                        signInForm.clearForm();
                        signInForm.validate().resetForm();

                        showErrorMsg(signInForm, 'success', 'Cool! Password recovery instruction has been sent to your email.');
                    }, 2000);
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
        }
    };
}();

//== Class Initialization
jQuery(document).ready(function() {
    SnippetLogin.init();
});