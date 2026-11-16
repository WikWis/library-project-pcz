import {SaveSubject} from "../save/save-subject.ts";
import type {UpdatePublisherDTO} from "../../../dto/publisher/update-publisher-dto.ts";
import {PublisherService, type PublisherServiceInterface} from "../../../services/publisher-service.ts";
import type {PublisherDTO} from "../../../dto/publisher/publisher-dto.ts";

export class PublisherUpdateSubject extends SaveSubject<UpdatePublisherDTO> {

    private readonly publisherService: PublisherServiceInterface = new PublisherService();

    async saveElement(element: UpdatePublisherDTO): Promise<boolean> {
        const savedPublisher: PublisherDTO | undefined = await this.publisherService.Update(element.publisherId, element);

        this._isCreated = savedPublisher !== undefined
        this.notify()

        return this._isCreated;
    }

}