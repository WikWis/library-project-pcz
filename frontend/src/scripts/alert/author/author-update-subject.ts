import {SaveSubject} from "../save/save-subject.ts";
import {AuthorService, type AuthorServiceInterface} from "../../../services/author-service.ts";
import type {AuthorDTO} from "../../../dto/author/author-dto.ts";
import type {UpdateAuthorDTO} from "../../../dto/author/update-author-dto.ts";

export class AuthorUpdateSubject extends SaveSubject<UpdateAuthorDTO> {

    async saveElement(element: UpdateAuthorDTO): Promise<boolean> {
        const authorService: AuthorServiceInterface = new AuthorService();
        const savedAuthor: AuthorDTO | undefined = await authorService.Update(element.authorId, element);

        this._isCreated = savedAuthor !== undefined
        this.notify()

        return this._isCreated;
    }

}