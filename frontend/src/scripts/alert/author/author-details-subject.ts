import {AuthorService, type AuthorServiceInterface} from "../../../services/author-service.ts";
import type {AuthorDTO} from "../../../dto/author/author-dto.ts";
import {DetailsSubject} from "../details/details-subject.ts";

export class AuthorDetailsSubject extends DetailsSubject<AuthorDTO> {

    private readonly authorService: AuthorServiceInterface = new AuthorService();

    async getDetailsElement(id: number): Promise<AuthorDTO | undefined> {
        this._element = await this.authorService.Details(id);

        this._gotElement = this._element !== undefined;
        this.notify();

        return this._element;
    }
}