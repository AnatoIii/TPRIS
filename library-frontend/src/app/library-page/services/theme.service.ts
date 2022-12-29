import { Injectable } from "@angular/core";

enum Theme {
    Light = 'light',
    Dark = 'dark'
}

@Injectable({
    providedIn: 'root'
  })
export class ThemeService {
    private setTheme(theme: Theme) {
        document.documentElement.setAttribute('theme', theme);
    }

    initTheme() {
        const theme = localStorage.getItem('theme');

        this.setTheme(this.stringToTheme(theme || ''));
    }
    
    private stringToTheme(theme: string) {
        if (theme == 'light') {
            return Theme.Light;
        } else {
            return Theme.Dark;
        }
    }
}