import type {CreateAuthorDTO} from "../../../dto/author/create-author-dto.ts";
import {SaveSubject} from "../save/save-subject.ts";
import {AuthorService, type AuthorServiceInterface} from "../../../services/author-service.ts";
import type {AuthorDTO} from "../../../dto/author/author-dto.ts";

export class AuthorSaveSubject extends SaveSubject<CreateAuthorDTO> {

    async saveElement(element: CreateAuthorDTO): Promise<boolean> {
        const authorService: AuthorServiceInterface = new AuthorService();
        const savedAuthor: AuthorDTO | undefined = await authorService.Create(element);

        this._isCreated = savedAuthor !== undefined
        this.notify()

        return this._isCreated;
    }

}