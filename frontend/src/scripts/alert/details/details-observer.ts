import type {Observer} from "../observer.ts";

export class DetailsObserver<T> implements Observer<T> {

    private readonly whenElementGotten: (element: T) => void;

    constructor(whenElementGotten: (element: T) => void) {
        this.whenElementGotten = whenElementGotten;
    }

    update(element: T): void {
        this.whenElementGotten(element);
    }
}