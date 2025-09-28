import { HttpInterceptorFn, HttpErrorResponse, HttpEvent } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

export const AuthInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const toastr = inject(ToastrService);
  const token = auth.getToken();

  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  return next(req).pipe(
    catchError((error: any) => {
      if (error instanceof HttpErrorResponse) {
        let message = 'An error occurred';
        switch (error.status) {
          case 0:
            message = 'Error de red, no es posible acceder al servidor';
            break;
          case 400:
            message = 'Los datos enviados no son correctos, favor de revisar';
            break;
          case 401:
            toastr.error('Por favor inicie sesión para continuar', 'Error');
            auth.logout();
            break;
          case 403:
            message = 'Prohibido, no tiene permisos suficientes para esta opción';
            break;
          case 404:
            message = 'No se ha encontrado el recurso que intenta acceder';
            break;
          case 500:
            message = 'Ha ocurrido un error en el servidor, contacte a soporte';
            break;
          default:
            message = `Error inesperado: ${error.status}`;
        }
        toastr.error(message, 'Error');
      } else {
        toastr.error('Error desconocido: ' + error, 'Error');
      }
      return throwError(() => error);
    })
  ) as unknown as Observable<HttpEvent<any>>;
};