import type {TitleDTO} from "../../../dto/title/title-dto.ts";
import {TitleService, type TitleServiceInterface} from "../../../services/title-service.ts";
import {DetailsSubject} from "../details/details-subject.ts";

export class TitleDetailsSubject extends DetailsSubject<TitleDTO | undefined> {

    async getDetailsElement(id: Number): Promise<TitleDTO | undefined> {
        const titleService: TitleServiceInterface = new TitleService();

        this._element = await titleService.Details(id);

        this._gotElement = this._element !== undefined;
        this.notify();

        return this._element;
    }

}