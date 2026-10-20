import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { TokenInterceptor } from './interceptor-token';
import { routes } from './app.routes';
import { MatNativeDateModule } from '@angular/material/core';
import { importProvidersFrom } from '@angular/core';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors([TokenInterceptor])),
    importProvidersFrom(MatNativeDateModule)
  ],
};
