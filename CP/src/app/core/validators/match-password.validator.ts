import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function MatchPasswordValidator(matchingControlName: string): ValidatorFn {
    return (c: AbstractControl): ValidationErrors | null => {
        let matchingControl = c.parent?.get(matchingControlName)
        if (
            matchingControl?.errors &&
            !matchingControl?.errors['matchPassword']
        ) {
            return null;
        }

        return c.value !== matchingControl?.value ? { matchPassword: true } : null;
    };
}