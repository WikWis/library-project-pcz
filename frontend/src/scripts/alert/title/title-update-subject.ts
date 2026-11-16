import type {UpdateTitleDTO} from "../../../dto/title/update-title-dto.ts";
import {TitleService, type TitleServiceInterface} from "../../../services/title-service.ts";
import {SaveSubject} from "../save/save-subject.ts";

export class TitleUpdateSubject extends SaveSubject<UpdateTitleDTO> {

    async saveElement(element: UpdateTitleDTO): Promise<boolean> {
        const titleService: TitleServiceInterface = new TitleService();
        const savedTitle = await titleService.Update(element.titleId, element);

        this._isCreated = savedTitle !== undefined;
        this.notify();

        return this._isCreated;
    }

}