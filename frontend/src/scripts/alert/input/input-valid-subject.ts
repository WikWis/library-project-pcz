import type {Observer} from "../observer.ts";
import type {Subject} from "../subject.ts";

export class InputValidSubject implements Subject<Boolean> {

    private _areValid: boolean = false;

    private observers: Observer<Boolean>[] = [];

    get areValid(): boolean {
        return this._areValid;
    }

    attach(observer: Observer<Boolean>): void {
        if (this.observers.includes(observer)) {
            return;
        }

        this.observers.push(observer);
    }

    detach(observer: Observer<Boolean>): void {
        const observerIndex = this.observers.indexOf(observer);

        if (observerIndex === -1) {
            return;
        }

        this.observers.splice(observerIndex, 1);
    }

    notify(): void {
        for (const observer of this.observers) {
            observer.update(this._areValid);
        }
    }

    areInputsValid(inputs: HTMLInputElement[]): boolean {
        inputs.forEach(input => {
            input.classList.toggle(
                "is-invalid",
                input.value.trim().length === 0
            );
        });

        this._areValid = inputs.map(input => input.value.trim().length !== 0)
            .reduce((previousValue, currentValue) => previousValue && currentValue)

        this.notify();

        return this._areValid;
    }

}