import { Injectable } from '@angular/core';
import { DateTimeRepository } from '../Repository/date-time.repository';
import { Observable } from 'rxjs';
import { inject } from '@angular/core';
import { DateTimeResponse } from '../Models/date-time.response';

@Injectable({
  providedIn: 'root'
})
export class DateTimeService {
  private repository = inject(DateTimeRepository);

  public SetDateTime(data: { dateTime: string }): Observable<DateTimeResponse> {
    return this.repository.SetDateTime(data);
  }

  public GetDateTime(): Observable<DateTimeResponse> {
    return this.repository.GetDateTime();
  }
}
