import type {Observer} from "../observer.ts";
import type {Subject} from "../subject.ts";

export abstract class SaveSubject<T> implements Subject<Boolean> {

    protected _isCreated: boolean = false;

    private observers: Observer<Boolean>[] = [];

    get isCreated(): boolean {
        return this._isCreated;
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
            observer.update(this.isCreated);
        }
    }

    abstract saveElement(element: T): Promise<boolean>;

}