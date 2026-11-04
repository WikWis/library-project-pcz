import type {CreateAuthorDTO} from "../../../dto/author/create-author-dto.ts";
import {AuthorService, type AuthorServiceInterface} from "../../../services/author-service.ts";
import type {AuthorDTO} from "../../../dto/author/author-dto.ts";

function createAuthorDTO(nameInput: HTMLInputElement, surnameInput: HTMLInputElement): CreateAuthorDTO {
    return {
        name: nameInput.value.trim(),
        surname: surnameInput.value.trim(),
    } as CreateAuthorDTO;
}

async function saveAuthorDTO(authorDTO: CreateAuthorDTO): Promise<boolean> {
    const authorService: AuthorServiceInterface = new AuthorService();
    const savedAuthor: AuthorDTO | undefined = await authorService.Create(authorDTO);

    return savedAuthor !== undefined;
}

function setUpAlerts(isSaved: boolean, successAlert: HTMLDivElement, dangerAlert: HTMLDivElement) {
    successAlert.classList.toggle("d-none", !isSaved);
    dangerAlert.classList.toggle("d-none", isSaved);
}

function validateIsInputCorrect(nameInput: HTMLInputElement, surnameInput: HTMLInputElement): boolean {
    const isNameCorrect: boolean = nameInput.value.trim().length !== 0;
    const isSurnameCorrect: boolean = surnameInput.value.trim().length !== 0;

    nameInput.classList.toggle("is-invalid", !isNameCorrect);
    surnameInput.classList.toggle("is-invalid", !isSurnameCorrect);

    return isNameCorrect && isSurnameCorrect;
}

export async function init(): Promise<void> {
    const nameInput: HTMLInputElement | null = document.querySelector<HTMLInputElement>('#author-name-input');
    const surnameInput: HTMLInputElement | null = document.querySelector<HTMLInputElement>('#author-surname-input');
    const addButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>('#submit-author-button');

    const successAlert: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#author-success-alert");
    const dangerAlert: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#author-danger-alert");

    if (nameInput === null || surnameInput === null || addButton === null || successAlert === null || dangerAlert === null) {
        throw new Error("Not every element is needed!");
    }

    addButton.addEventListener('click', async (): Promise<void> => {
        const areInputsCorrect: boolean = validateIsInputCorrect(nameInput, surnameInput);

        if (!areInputsCorrect) {
            dangerAlert.classList.remove("d-none");
            return;
        }

        const authorDto: CreateAuthorDTO = createAuthorDTO(nameInput, surnameInput);

        const isSaved: boolean = await saveAuthorDTO(authorDto);

        setUpAlerts(isSaved, successAlert, dangerAlert);

        if (isSaved) {
            nameInput.value = "";
            surnameInput.value = "";
        }

    });
}