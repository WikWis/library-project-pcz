import type {Observer} from "../observer.ts";
import type {Subject} from "../subject.ts";

export abstract class DetailsSubject<T> implements Subject<T | undefined> {

    protected _gotElement: Boolean = false;
    protected _element: T | undefined;

    private observers: Observer<T | undefined>[] = [];

    get gotElement(): Boolean {
        return this._gotElement;
    }

    attach(observer: Observer<T | undefined>): void {
        if (this.observers.includes(observer)) {
            return;
        }

        this.observers.push(observer);
    }

    detach(observer: Observer<T | undefined>): void {
        const observerIndex = this.observers.indexOf(observer);
        if (observerIndex === -1) {
            return;
        }

        this.observers.splice(observerIndex, 1);
    }

    notify(): void {
        for (const observer of this.observers) {
            observer.update(this._element);
        }
    }

    abstract getDetailsElement(id: number): Promise<T | undefined>;

}