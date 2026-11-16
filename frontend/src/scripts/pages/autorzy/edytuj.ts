import type {AuthorDTO} from "../../../dto/author/author-dto.ts";
import {InputValidSubject} from "../../alert/input/input-valid-subject.ts";
import {InputValidObserver} from "../../alert/input/input-valid-observer.ts";
import {SaveObserver} from "../../alert/save/save-observer.ts";
import {AuthorUpdateSubject} from "../../alert/author/author-update-subject.ts";
import type {UpdateAuthorDTO} from "../../../dto/author/update-author-dto.ts";
import {DetailsObserver} from "../../alert/details/details-observer.ts";
import {AuthorDetailsSubject} from "../../alert/author/author-details-subject.ts";

const params: URLSearchParams = new URLSearchParams(window.location.search);
const pathId: string | null = params.get("id");

const nameInput: HTMLInputElement | null = document.querySelector<HTMLInputElement>('#author-name-input');
const surnameInput: HTMLInputElement | null = document.querySelector<HTMLInputElement>('#author-surname-input');
const submitAuthorButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>('#submit-author-button');

const creationSuccessAlert: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#author-success-alert");
const errorAlert: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#author-danger-alert");

const notFoundMessageDiv: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#not-found-message")
const contentDiv: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#content")

function createAuthorDTO(id: Number, nameInput: HTMLInputElement, surnameInput: HTMLInputElement): UpdateAuthorDTO {
    return {
        authorId: id,
        name: nameInput.value.trim(),
        surname: surnameInput.value.trim(),
    } as UpdateAuthorDTO;
}

export async function init(): Promise<void> {

    if (nameInput === null || surnameInput === null || submitAuthorButton === null || creationSuccessAlert === null || errorAlert === null || notFoundMessageDiv === null || contentDiv === null) {
        throw new Error("Not every element is present!");
    }

    // Get Author Details
    const authorDetailsSubject = new AuthorDetailsSubject();
    const authorDetailsObserver = new DetailsObserver<AuthorDTO | undefined>((authorDto) => {
            if (authorDto === undefined) {
                notFoundMessageDiv.hidden = false;
                contentDiv.hidden = true;
                return;
            }
            nameInput.value = authorDto.name;
            surnameInput.value = authorDto.surname;
            notFoundMessageDiv.hidden = true;
            contentDiv.hidden = false;
        }
    );
    authorDetailsSubject.attach(authorDetailsObserver);

    const id: number = Number.parseInt(pathId ?? "", 10);
    if (!Number.isInteger(id) || id <= 0) {
        notFoundMessageDiv.hidden = false;
        contentDiv.hidden = true;
        return;
    }
    await authorDetailsSubject.getDetailsElement(id);
    if (!authorDetailsSubject.gotElement) {
        return;
    }

    // Preparing author update
    const authorUpdateSubject: AuthorUpdateSubject = new AuthorUpdateSubject();
    const authorUpdateObserver: SaveObserver = new SaveObserver((wasAuthorSaved: Boolean) => {
        creationSuccessAlert.classList.toggle("d-none", !wasAuthorSaved);
        errorAlert.classList.toggle("d-none", wasAuthorSaved.valueOf());
    });
    authorUpdateSubject.attach(authorUpdateObserver);

    // Input validation
    const inputValidSubject: InputValidSubject = new InputValidSubject();
    const inputValidationObserver: InputValidObserver = new InputValidObserver((areInputsValid: Boolean) => {
        if (!areInputsValid) {
            errorAlert.classList.toggle("d-none", areInputsValid);
        }
    });
    inputValidSubject.attach(inputValidationObserver);

    // Submit author
    submitAuthorButton.onclick = async (): Promise<void> => {
        const areInputsValid: boolean = inputValidSubject.areInputsValid([
            surnameInput,
            nameInput
        ]);
        if (!areInputsValid) {
            return;
        }
        const authorDto: UpdateAuthorDTO = createAuthorDTO(id, nameInput, surnameInput);
        await authorUpdateSubject.saveElement(authorDto);
    };

}