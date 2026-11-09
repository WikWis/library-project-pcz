import type {CreateTitleDTO} from "../../../dto/title/create-title-dto.ts";
import {TitleService, type TitleServiceInterface} from "../../../services/title-service.ts";
import type {TitleDTO} from "../../../dto/title/title-dto.ts";
import {SaveSubject} from "../save/save-subject.ts";

export class TitleSaveSubject extends SaveSubject<CreateTitleDTO> {
    async saveElement(element: CreateTitleDTO): Promise<boolean> {
        const titleService: TitleServiceInterface = new TitleService();
        const savedTitle: TitleDTO | undefined = await titleService.Create(element);

        this._isCreated = savedTitle !== undefined
        this.notify()

        return this._isCreated;
    }
}

