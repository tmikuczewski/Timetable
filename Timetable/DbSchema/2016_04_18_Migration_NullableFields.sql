ALTER TABLE public.classes
   ALTER COLUMN tutor DROP NOT NULL;

ALTER TABLE public.classrooms
   ALTER COLUMN administrator DROP NOT NULL;

ALTER TABLE public.students
   ALTER COLUMN class DROP NOT NULL;
