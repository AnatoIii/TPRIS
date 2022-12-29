import { Component } from '@angular/core';
import { AuthService } from './library-page/services/auth.service';
import { ThemeService } from './library-page/services/theme.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Library';

  constructor(
    themeService: ThemeService,
    authService: AuthService
  ) {
    authService.checkToken();
    themeService.initTheme();
  }
}
