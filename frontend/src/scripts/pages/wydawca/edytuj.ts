import type {PublisherDTO} from "../../../dto/publisher/publisher-dto.ts";
import type {UpdatePublisherDTO} from "../../../dto/publisher/update-publisher-dto.ts";

import {DetailsObserver} from "../../alert/details/details-observer.ts";
import {PublisherDetailsSubject} from "../../alert/publisher/publisher-details-subject.ts";

import {InputValidSubject} from "../../alert/input/input-valid-subject.ts";
import {InputValidObserver} from "../../alert/input/input-valid-observer.ts";

import {SaveObserver} from "../../alert/save/save-observer.ts";
import {PublisherUpdateSubject} from "../../alert/publisher/publisher-update-subject.ts";


const params: URLSearchParams = new URLSearchParams(window.location.search);
const pathId: string | null = params.get("id");

const nameInput: HTMLInputElement | null = document.querySelector<HTMLInputElement>('#publisher-name-input');

const submitPublisherButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>('#submit-publisher-button');

const creationSuccessAlert: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#publisher-success-alert");

const errorAlert: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#publisher-danger-alert");

const notFoundMessageDiv: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#not-found-message");

const contentDiv: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#content");


function createPublisherDTO(id: number, nameInput: HTMLInputElement): UpdatePublisherDTO {
    return {
        publisherId: id,
        name: nameInput.value.trim()
    } as UpdatePublisherDTO;
}


export async function init(): Promise<void> {

    if (nameInput === null || submitPublisherButton === null || creationSuccessAlert === null || errorAlert === null || notFoundMessageDiv === null || contentDiv === null) {
        throw new Error("Not every element is present!");
    }

    // Getting publisher details
    const publisherDetailsSubject = new PublisherDetailsSubject();
    const publisherDetailsObserver = new DetailsObserver<PublisherDTO | undefined>(
        (publisherDto) => {
            if (publisherDto === undefined) {
                notFoundMessageDiv.hidden = false;
                contentDiv.hidden = true;
                return;
            }
            nameInput.value = publisherDto.name;
            notFoundMessageDiv.hidden = true;
            contentDiv.hidden = false;
        }
    );
    publisherDetailsSubject.attach(publisherDetailsObserver);

    const id: number = Number.parseInt(pathId ?? "", 10);
    if (!Number.isInteger(id) || id <= 0) {
        notFoundMessageDiv.hidden = false;
        contentDiv.hidden = true;
        return;
    }
    await publisherDetailsSubject.getDetailsElement(id);
    if (!publisherDetailsSubject.gotElement) {
        return;
    }

    // Preparing publisher update
    const publisherUpdateSubject: PublisherUpdateSubject = new PublisherUpdateSubject();
    const publisherUpdateObserver: SaveObserver = new SaveObserver(
        (wasPublisherUpdated: Boolean) => {
            creationSuccessAlert.classList.toggle("d-none", !wasPublisherUpdated);
            errorAlert.classList.toggle("d-none", wasPublisherUpdated.valueOf());
        }
    );
    publisherUpdateSubject.attach(publisherUpdateObserver);

    // Input validation
    const inputValidSubject: InputValidSubject = new InputValidSubject();
    const inputValidationObserver: InputValidObserver = new InputValidObserver(
        (areInputsValid: Boolean) => {
            if (!areInputsValid) {
                errorAlert.classList.toggle("d-none", areInputsValid);
            }
        }
    );

    inputValidSubject.attach(inputValidationObserver);

    // Submit publisher

    submitPublisherButton.onclick = async (): Promise<void> => {
        const areInputsValid: boolean = inputValidSubject.areInputsValid([
            nameInput
        ]);

        if (!areInputsValid) {
            return;
        }

        const publisherDTO: UpdatePublisherDTO =
            createPublisherDTO(id, nameInput);
        await publisherUpdateSubject.saveElement(publisherDTO);
    };
}