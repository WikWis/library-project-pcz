import type {CreateAuthorDTO} from "../../../dto/author/create-author-dto.ts";
import {AuthorSaveSubject} from "../../alert/author/author-save-subject.ts";
import {InputValidSubject} from "../../alert/input/input-valid-subject.ts";
import {InputValidObserver} from "../../alert/input/input-valid-observer.ts";
import {SaveObserver} from "../../alert/save/save-observer.ts";

const nameInput: HTMLInputElement | null = document.querySelector<HTMLInputElement>('#author-name-input');
const surnameInput: HTMLInputElement | null = document.querySelector<HTMLInputElement>('#author-surname-input');
const createAuthorButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>('#submit-author-button');

const creationSuccessAlert: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#author-success-alert");
const errorAlert: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#author-danger-alert");

function createAuthorDTO(nameInput: HTMLInputElement, surnameInput: HTMLInputElement): CreateAuthorDTO {
    return {
        name: nameInput.value.trim(),
        surname: surnameInput.value.trim(),
    } as CreateAuthorDTO;
}

export async function init(): Promise<void> {

    if (nameInput === null || surnameInput === null || createAuthorButton === null || creationSuccessAlert === null || errorAlert === null) {
        throw new Error("Not every element is present!");
    }

    const authorSaveSubject: AuthorSaveSubject = new AuthorSaveSubject()
    const authorSaveObserver: SaveObserver = new SaveObserver((wasAuthorCreated: Boolean) => {
        if (wasAuthorCreated) {
            nameInput.value = "";
            surnameInput.value = "";
        }
        creationSuccessAlert.classList.toggle("d-none", !wasAuthorCreated);
        errorAlert.classList.toggle("d-none", wasAuthorCreated.valueOf());
    });
    authorSaveSubject.attach(authorSaveObserver)

    const inputValidSubject: InputValidSubject = new InputValidSubject();
    const inputValidationObserver: InputValidObserver = new InputValidObserver((areInputsValid: Boolean) => {
        if (!areInputsValid) {
            errorAlert.classList.toggle("d-none", areInputsValid);
        }
    });
    inputValidSubject.attach(inputValidationObserver);

    createAuthorButton.click = async (): Promise<void> => {
        const areInputsValid: boolean = inputValidSubject.areInputsValid([
            surnameInput,
            nameInput
        ]);

        if (!areInputsValid) {
            return;
        }

        // Save Author
        const authorDto: CreateAuthorDTO = createAuthorDTO(nameInput, surnameInput);
        await authorSaveSubject.saveElement(authorDto)
    };
}