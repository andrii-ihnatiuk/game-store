import { Observable } from "rxjs";
import { Image } from "src/app/models/image.model";

export interface ImageSelectData {
    fetchImages: () => Observable<Image[]>;
    uploadImages: (images: FormData) => Observable<void>;
    multiSelect: boolean;
} 