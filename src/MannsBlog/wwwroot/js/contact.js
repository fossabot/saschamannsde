// MIT License
//
// Copyright (c) 2022 Sascha Manns
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

// contact.js
(function (Vue, VeeValidate) {

    Vue.use(VeeValidate);

    var app = new Vue({
        el: "#contact-form",
        data: {
            mail: {
                name: "",
                email: "",
                subject: "Pick One...",
                message: "",
                recaptcha: ""
            },
            subjects: [
                "Pick One...",
                "Talk",
                "Job Offer",
                "Other"
            ],
            errorMessage: "",
            statusMessage: ""
        },
        computed: {
            isPristine: function () {
                var val = (this.mail.name === "" || this.mail.email === "" || this.mail.message === "" || this.mail.subject === this.subjects[0]);
                return val;
            }
        },
        methods: {
            onSubmit: function () {

                var me = this;

                me.statusMessage = "";

                var captcha = document.getElementById("g-recaptcha-response");
                if (captcha && captcha.value) {
                    me.mail.recaptcha = captcha.value;
                } else {
                    me.statusMessage = "Please confirm Captcha";
                    return;
                }

                // Validate All returns a promise and provides the validation result.
                this.$validator.validateAll().then(function (success) {
                    if (!success) {
                        me.errorMessage = "Please fix one or more validation errors...";
                        return;
                    }
                    me.statusMessage = "Sending...";
                    me.errorMessage = "";

                    me.$http.post("/contact", me.mail)
                        .then(function () {
                            me.mail.name = "";
                            me.mail.email = "";
                            me.mail.subject = me.subjects[0];
                            me.mail.messsage = "";
                            this.$validator.reset();
                            me.statusMessage = "Message Sent...";
                        }, function (response) {
                            me.statusMessage = "";
                            if (response.body && response.body.reason) {
                                me.errorMessage = response.body.reason;
                            }
                            else {
                                me.errorMessage = "Failed to send message!";
                            }
                        });
                });
            }
        },
        created: function () {
            this.$set(this, 'errors', this.$validator.errorBag);
        }
    });

})(Vue, VeeValidate);