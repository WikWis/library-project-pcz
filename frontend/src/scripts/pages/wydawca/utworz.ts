import type {CreatePublisherDTO} from "../../../dto/publisher/create-publisher-dto.ts";
import {SaveObserver} from "../../alert/save/save-observer.ts";
import {InputValidSubject} from "../../alert/input/input-valid-subject.ts";
import {InputValidObserver} from "../../alert/input/input-valid-observer.ts";
import type {SaveSubject} from "../../alert/save/save-subject.ts";
import {PublisherSaveSubject} from "../../alert/publisher/publisher-save-subject.ts";

const nameInput: HTMLInputElement | null = document.querySelector<HTMLInputElement>('#publisher-name-input');

const createPublisherButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>('#submit-publisher-button');

const creationSuccessAlert: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#publisher-success-alert");
const errorAlert: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#publisher-danger-alert");

function createPublisherDTO(nameInput: HTMLInputElement): CreatePublisherDTO {
    return {
        name: nameInput.value.trim()
    } as CreatePublisherDTO;
}

export async function init(): Promise<void> {

    if (nameInput === null || creationSuccessAlert === null || errorAlert === null || createPublisherButton === null) {
        throw new Error("Not every element is present!");
    }

    const publisherSaveSubject: SaveSubject<CreatePublisherDTO> = new PublisherSaveSubject()
    const publisherSaveObserver: SaveObserver = new SaveObserver((wasAuthorCreated: Boolean) => {
        if (wasAuthorCreated) {
            nameInput.value = "";
        }
        creationSuccessAlert.classList.toggle("d-none", !wasAuthorCreated);
        errorAlert.classList.toggle("d-none", wasAuthorCreated.valueOf());
    });
    publisherSaveSubject.attach(publisherSaveObserver)

    const inputValidSubject: InputValidSubject = new InputValidSubject();
    const inputValidationObserver: InputValidObserver = new InputValidObserver((areInputsValid: Boolean) => {
        if (!areInputsValid) {
            errorAlert.classList.toggle("d-none", areInputsValid);
        }
    });
    inputValidSubject.attach(inputValidationObserver);

    createPublisherButton.onclick = async (): Promise<void> => {

        const areInputsValid: boolean = inputValidSubject.areInputsValid([
            nameInput
        ]);

        if (!areInputsValid) {
            return;
        }

        // Save Publisher
        const publisherDTO: CreatePublisherDTO = createPublisherDTO(nameInput);
        await publisherSaveSubject.saveElement(publisherDTO)
    }

}