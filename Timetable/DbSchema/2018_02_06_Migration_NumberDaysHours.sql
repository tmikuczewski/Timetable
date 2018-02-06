ALTER TABLE public.days
   ADD COLUMN "number" integer NOT NULL DEFAULT 0;

UPDATE public.days SET number = 1 WHERE id = 1;
UPDATE public.days SET number = 2 WHERE id = 2;
UPDATE public.days SET number = 3 WHERE id = 3;
UPDATE public.days SET number = 4 WHERE id = 4;
UPDATE public.days SET number = 5 WHERE id = 5;
   
ALTER TABLE public.hours
   RENAME "hour" TO "begin";

ALTER TABLE public.hours
   ADD COLUMN "end" time without time zone NOT NULL DEFAULT '00:00:00';

UPDATE public.hours SET "end" = '08:15:00' WHERE id = 1;
UPDATE public.hours SET "end" = '09:15:00' WHERE id = 2;
UPDATE public.hours SET "end" = '10:15:00' WHERE id = 3;
UPDATE public.hours SET "end" = '11:15:00' WHERE id = 4;
UPDATE public.hours SET "end" = '12:30:00' WHERE id = 5;
UPDATE public.hours SET "end" = '13:30:00' WHERE id = 6;
UPDATE public.hours SET "end" = '14:30:00' WHERE id = 7;
UPDATE public.hours SET "end" = '15:30:00' WHERE id = 8;
 
ALTER TABLE public.hours
   ADD COLUMN "number" integer NOT NULL DEFAULT 0;

UPDATE public.hours SET number = 0 WHERE id = 1;
UPDATE public.hours SET number = 1 WHERE id = 2;
UPDATE public.hours SET number = 2 WHERE id = 3;
UPDATE public.hours SET number = 3 WHERE id = 4;
UPDATE public.hours SET number = 4 WHERE id = 5;
UPDATE public.hours SET number = 5 WHERE id = 6;
UPDATE public.hours SET number = 6 WHERE id = 7;
UPDATE public.hours SET number = 7 WHERE id = 8;
