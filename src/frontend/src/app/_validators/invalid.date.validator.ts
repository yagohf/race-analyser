import { ValidatorFn, AbstractControl } from '@angular/forms';

export function invalidDateValidatorFn(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
        if (!control.value) {
            return null;
        }

        const date = new Date(control.value);
        if (date) {
            const invalidDate = date.getFullYear() < 1900 || date.getFullYear() > new Date().getFullYear();
            return invalidDate ? { 'invalidDate': { value: control.value } } : null;
        }
        else {
            return null;
        }
    };
}