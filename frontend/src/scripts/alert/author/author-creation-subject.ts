import type {Observer} from "../observer.ts";
import type {Subject} from "../subject.ts";
import type {CreateAuthorDTO} from "../../../dto/author/create-author-dto.ts";
import {AuthorService, type AuthorServiceInterface} from "../../../services/author-service.ts";
import type {AuthorDTO} from "../../../dto/author/author-dto.ts";

export class AuthorCreationSubject implements Subject<Boolean> {

    private _isCreated: boolean = false;

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

    async createAuthor(authorDTO: CreateAuthorDTO): Promise<boolean> {
        const authorService: AuthorServiceInterface = new AuthorService();
        const savedAuthor: AuthorDTO | undefined = await authorService.Create(authorDTO);

        this._isCreated = savedAuthor !== undefined
        this.notify()

        return this._isCreated;
    }

}