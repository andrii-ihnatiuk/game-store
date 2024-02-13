import { FormControl, FormGroupDirective, NgForm } from "@angular/forms";
import { ErrorStateMatcher } from "@angular/material/core";

export class InputErrorStateMatcher implements ErrorStateMatcher {
    disableValidation = true;
  
    isErrorState(
      control: FormControl | null,
      form: FormGroupDirective | NgForm | null
    ): boolean {
      return !this.disableValidation && !!(control && control.invalid);
    }
  }