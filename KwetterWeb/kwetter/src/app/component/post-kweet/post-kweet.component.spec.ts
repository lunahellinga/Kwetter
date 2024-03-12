import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostKweetComponent } from './post-kweet.component';

describe('PostKweetComponent', () => {
  let component: PostKweetComponent;
  let fixture: ComponentFixture<PostKweetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PostKweetComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PostKweetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
