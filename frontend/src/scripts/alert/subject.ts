import type {Observer} from "./observer.ts";

export interface Subject<T> {
    attach(observer: Observer<T>): void;

    detach(observer: Observer<T>): void;

    notify(): void;
}