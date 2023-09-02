import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup = new FormGroup({});
  maxDate: Date = new Date();
  validationErrors: string[] | undefined;

  constructor(
    private accountService: AccountService,
    private fb: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  register() {
    const dob = this.GetDateOnly(
      this.registerForm.controls['dateOfBirth'].value
    );
    const values = {
      ...this.registerForm.value,
      dateOfBirth: this.GetDateOnly(dob),
    };
    this.accountService.register(values).subscribe({
      next: () => {
        this.router.navigateByUrl('/members');
      },
      error: (error) => console.log(error),
    });
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      username: ['', [Validators.required]],
      gender: ['male'],
      knownAs: ['', [Validators.required]],
      city: ['', [Validators.required]],
      dateOfBirth: ['', [Validators.required]],
      country: ['', [Validators.required]],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(20),
        ],
      ],
      confirmPassword: [
        '',
        [Validators.required, this.matchValues('password')],
      ],
    });
    this.registerForm.controls['password'].valueChanges.subscribe(() => {
      this.registerForm.controls['confirmPassword'].updateValueAndValidity();
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === this.registerForm.get(matchTo)?.value
        ? null
        : { isMatching: true };
    };
  }

  // Helper function to display error messages in the HTML
  getErrorMessage(controlName: string) {
    const control = this.registerForm.get(controlName);
    if (control?.hasError('required')) {
      return 'This field is required';
    }
    if (controlName === 'password' && control?.hasError('minlength')) {
      return 'Password should have at least 4 characters';
    }
    if (controlName === 'password' && control?.hasError('maxlength')) {
      return 'Password is too long (maximum 20 characters)';
    }
    if (controlName === 'confirmPassword' && control?.hasError('isMatching')) {
      return 'Passwords do not match';
    }
    return '';
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

  private GetDateOnly(dob: string | undefined) {
    if (!dob) return;
    let theDob = new Date(dob);
    return new Date(
      theDob.setMinutes(theDob.getMinutes() - theDob.getTimezoneOffset())
    )
      .toISOString()
      .slice(0, 10);
  }
}
