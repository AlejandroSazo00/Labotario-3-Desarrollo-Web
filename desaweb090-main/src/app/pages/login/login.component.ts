import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Login } from '../../models/dto/login';
import { Router } from '@angular/router';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  form: FormGroup;
  error = '';

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router
  ) {
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  ngOnInit() {
    if (this.auth.isAuthenticated()) {
      this.router.navigate(['/persons']);
    }
  }

  onSubmit(): void {
    if (this.form.valid) {
      const login: Login = this.form.value;
      this.auth.login(login).subscribe({
        next: (res) => {
          this.auth.saveLoginData(res);
          this.router.navigate(['/persons']);
        },
        error: (err) => {
          this.error = 'Credenciales inválidas.';
          console.error(err);
        },
      });
    }
  }

}
