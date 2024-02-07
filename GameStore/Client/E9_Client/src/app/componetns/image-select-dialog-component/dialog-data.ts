import { Observable } from "rxjs";
import { Image } from "src/app/models/image.model";

export interface DialogData {
    fetchImages: () => Observable<Image[]>;
    uploadImages: (images: FormData) => Observable<void>;
    multiSelect: boolean;
} 