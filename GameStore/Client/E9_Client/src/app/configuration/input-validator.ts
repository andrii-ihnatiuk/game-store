import { ValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';

export class InputValidator {
  static getNumberValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }

      if (
        Number.parseInt(control.value) < 0 ||
        Number.parseFloat(control.value) != Number.parseInt(control.value)
      ) {
        return { invalidNumber: true };
      }

      return null;
    };
  }
}
