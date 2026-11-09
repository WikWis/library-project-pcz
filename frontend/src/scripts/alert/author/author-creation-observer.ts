import type {Observer} from "../observer.ts";

export class AuthorCreationObserver implements Observer<Boolean> {

    private readonly onAuthorCreated: (success: Boolean) => void;

    constructor(onAuthorCreated: (success: Boolean) => void) {
        this.onAuthorCreated = onAuthorCreated;
    }

    update(success: boolean): void {
        this.onAuthorCreated(success);
    }
}