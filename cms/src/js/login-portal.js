export class LoginPortal {
	constructor() {
		this.form = document.getElementById("login-form");
		if (!this.form) return;
		this.submitLogin = document.getElementById("submitLogin");
		[...this.form.elements].forEach((element) => {
			if (element.required) {
				element.addEventListener("input", () => {
					this.submitLogin.disabled = !this.form.checkValidity();
				});
			}
		});
	}
}
