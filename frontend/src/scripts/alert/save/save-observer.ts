import type {Observer} from "../observer.ts";

export class SaveObserver implements Observer<Boolean> {

    private readonly onObjectCreated: (success: Boolean) => void;

    constructor(onObjectCreated: (success: Boolean) => void) {
        this.onObjectCreated = onObjectCreated;
    }

    update(success: boolean): void {
        this.onObjectCreated(success);
    }
}