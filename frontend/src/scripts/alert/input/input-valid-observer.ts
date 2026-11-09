import type {Observer} from "../observer.ts";

export class InputValidObserver implements Observer<Boolean> {

    private readonly action: (areValid: Boolean) => void;

    constructor(action: (areValid: Boolean) => void) {
        this.action = action;
    }

    update(areValid: Boolean): void {
        this.action(areValid);
    }
}