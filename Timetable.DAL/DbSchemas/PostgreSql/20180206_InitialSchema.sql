--
-- PostgreSQL database dump
--

-- Dumped from database version 10.1
-- Dumped by pg_dump version 10.1

-- Started on 2018-02-06 20:20:14

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 1 (class 3079 OID 12924)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2882 (class 0 OID 0)
-- Dependencies: 1
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 196 (class 1259 OID 16394)
-- Name: classes; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE classes (
    id integer NOT NULL,
    year integer NOT NULL,
    code_name text,
    tutor character varying(11)
);


ALTER TABLE classes OWNER TO postgres;

--
-- TOC entry 197 (class 1259 OID 16400)
-- Name: classes_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE classes_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE classes_id_seq OWNER TO postgres;

--
-- TOC entry 2883 (class 0 OID 0)
-- Dependencies: 197
-- Name: classes_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE classes_id_seq OWNED BY classes.id;


--
-- TOC entry 198 (class 1259 OID 16402)
-- Name: classrooms; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE classrooms (
    id integer NOT NULL,
    name text NOT NULL,
    administrator character varying(11)
);


ALTER TABLE classrooms OWNER TO postgres;

--
-- TOC entry 199 (class 1259 OID 16408)
-- Name: classrooms_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE classrooms_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE classrooms_id_seq OWNER TO postgres;

--
-- TOC entry 2884 (class 0 OID 0)
-- Dependencies: 199
-- Name: classrooms_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE classrooms_id_seq OWNED BY classrooms.id;


--
-- TOC entry 200 (class 1259 OID 16410)
-- Name: days; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE days (
    id integer NOT NULL,
    name text NOT NULL,
    number integer NOT NULL
);


ALTER TABLE days OWNER TO postgres;

--
-- TOC entry 201 (class 1259 OID 16416)
-- Name: days_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE days_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE days_id_seq OWNER TO postgres;

--
-- TOC entry 2885 (class 0 OID 0)
-- Dependencies: 201
-- Name: days_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE days_id_seq OWNED BY days.id;


--
-- TOC entry 202 (class 1259 OID 16418)
-- Name: hours; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE hours (
    id integer NOT NULL,
    "begin" time without time zone NOT NULL,
    "end" time without time zone NOT NULL,
    number integer NOT NULL
);


ALTER TABLE hours OWNER TO postgres;

--
-- TOC entry 203 (class 1259 OID 16421)
-- Name: hours_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE hours_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE hours_id_seq OWNER TO postgres;

--
-- TOC entry 2886 (class 0 OID 0)
-- Dependencies: 203
-- Name: hours_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE hours_id_seq OWNED BY hours.id;


--
-- TOC entry 204 (class 1259 OID 16423)
-- Name: lessons; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE lessons (
    id integer NOT NULL,
    teacher character varying(11) NOT NULL,
    subject integer NOT NULL,
    class integer NOT NULL
);


ALTER TABLE lessons OWNER TO postgres;

--
-- TOC entry 205 (class 1259 OID 16426)
-- Name: lessons_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE lessons_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE lessons_id_seq OWNER TO postgres;

--
-- TOC entry 2887 (class 0 OID 0)
-- Dependencies: 205
-- Name: lessons_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE lessons_id_seq OWNED BY lessons.id;


--
-- TOC entry 206 (class 1259 OID 16428)
-- Name: lessons_places; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE lessons_places (
    lesson integer NOT NULL,
    classroom integer NOT NULL,
    day integer NOT NULL,
    hour integer NOT NULL
);


ALTER TABLE lessons_places OWNER TO postgres;

--
-- TOC entry 207 (class 1259 OID 16431)
-- Name: students; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE students (
    pesel character varying(11) NOT NULL,
    first_name text NOT NULL,
    last_name text NOT NULL,
    class integer
);


ALTER TABLE students OWNER TO postgres;

--
-- TOC entry 208 (class 1259 OID 16437)
-- Name: subjects; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE subjects (
    id integer NOT NULL,
    name text NOT NULL
);


ALTER TABLE subjects OWNER TO postgres;

--
-- TOC entry 209 (class 1259 OID 16443)
-- Name: subjects_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE subjects_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE subjects_id_seq OWNER TO postgres;

--
-- TOC entry 2888 (class 0 OID 0)
-- Dependencies: 209
-- Name: subjects_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE subjects_id_seq OWNED BY subjects.id;


--
-- TOC entry 210 (class 1259 OID 16445)
-- Name: teachers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE teachers (
    pesel character varying(11) NOT NULL,
    first_name text NOT NULL,
    last_name text NOT NULL
);


ALTER TABLE teachers OWNER TO postgres;

--
-- TOC entry 2718 (class 2604 OID 16451)
-- Name: classes id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY classes ALTER COLUMN id SET DEFAULT nextval('classes_id_seq'::regclass);


--
-- TOC entry 2719 (class 2604 OID 16452)
-- Name: classrooms id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY classrooms ALTER COLUMN id SET DEFAULT nextval('classrooms_id_seq'::regclass);


--
-- TOC entry 2720 (class 2604 OID 16453)
-- Name: days id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY days ALTER COLUMN id SET DEFAULT nextval('days_id_seq'::regclass);


--
-- TOC entry 2722 (class 2604 OID 16454)
-- Name: hours id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY hours ALTER COLUMN id SET DEFAULT nextval('hours_id_seq'::regclass);


--
-- TOC entry 2725 (class 2604 OID 16455)
-- Name: lessons id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons ALTER COLUMN id SET DEFAULT nextval('lessons_id_seq'::regclass);


--
-- TOC entry 2726 (class 2604 OID 16456)
-- Name: subjects id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY subjects ALTER COLUMN id SET DEFAULT nextval('subjects_id_seq'::regclass);


--
-- TOC entry 2728 (class 2606 OID 16458)
-- Name: classes classes_pk_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY classes
    ADD CONSTRAINT classes_pk_id PRIMARY KEY (id);


--
-- TOC entry 2730 (class 2606 OID 16460)
-- Name: classrooms classrooms_pk_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY classrooms
    ADD CONSTRAINT classrooms_pk_id PRIMARY KEY (id);


--
-- TOC entry 2732 (class 2606 OID 16462)
-- Name: days days_pk_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY days
    ADD CONSTRAINT days_pk_id PRIMARY KEY (id);


--
-- TOC entry 2734 (class 2606 OID 16464)
-- Name: hours hours_pk_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY hours
    ADD CONSTRAINT hours_pk_id PRIMARY KEY (id);


--
-- TOC entry 2736 (class 2606 OID 16466)
-- Name: lessons lessons_pk_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons
    ADD CONSTRAINT lessons_pk_id PRIMARY KEY (id);


--
-- TOC entry 2738 (class 2606 OID 16468)
-- Name: lessons_places lessons_places_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons_places
    ADD CONSTRAINT lessons_places_pk PRIMARY KEY (lesson, classroom, day, hour);


--
-- TOC entry 2740 (class 2606 OID 16470)
-- Name: students students_pk_pesel; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY students
    ADD CONSTRAINT students_pk_pesel PRIMARY KEY (pesel);


--
-- TOC entry 2742 (class 2606 OID 16472)
-- Name: subjects subjects_pk_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY subjects
    ADD CONSTRAINT subjects_pk_id PRIMARY KEY (id);


--
-- TOC entry 2744 (class 2606 OID 16474)
-- Name: teachers teachers_pk_pesel; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY teachers
    ADD CONSTRAINT teachers_pk_pesel PRIMARY KEY (pesel);


--
-- TOC entry 2745 (class 2606 OID 16475)
-- Name: classes classes_fk_tutor; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY classes
    ADD CONSTRAINT classes_fk_tutor FOREIGN KEY (tutor) REFERENCES teachers(pesel);


--
-- TOC entry 2746 (class 2606 OID 16480)
-- Name: classrooms classrooms_fk_administrator; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY classrooms
    ADD CONSTRAINT classrooms_fk_administrator FOREIGN KEY (administrator) REFERENCES teachers(pesel);


--
-- TOC entry 2747 (class 2606 OID 16485)
-- Name: lessons lessons_fk_class; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons
    ADD CONSTRAINT lessons_fk_class FOREIGN KEY (class) REFERENCES classes(id);


--
-- TOC entry 2748 (class 2606 OID 16490)
-- Name: lessons lessons_fk_subject; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons
    ADD CONSTRAINT lessons_fk_subject FOREIGN KEY (subject) REFERENCES subjects(id);


--
-- TOC entry 2749 (class 2606 OID 16495)
-- Name: lessons lessons_fk_teacher; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons
    ADD CONSTRAINT lessons_fk_teacher FOREIGN KEY (teacher) REFERENCES teachers(pesel);


--
-- TOC entry 2750 (class 2606 OID 16500)
-- Name: lessons_places lessons_places_fk_classroom; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons_places
    ADD CONSTRAINT lessons_places_fk_classroom FOREIGN KEY (classroom) REFERENCES classrooms(id);


--
-- TOC entry 2751 (class 2606 OID 16505)
-- Name: lessons_places lessons_places_fk_day; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons_places
    ADD CONSTRAINT lessons_places_fk_day FOREIGN KEY (day) REFERENCES days(id);


--
-- TOC entry 2752 (class 2606 OID 16510)
-- Name: lessons_places lessons_places_fk_hour; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons_places
    ADD CONSTRAINT lessons_places_fk_hour FOREIGN KEY (hour) REFERENCES hours(id);


--
-- TOC entry 2753 (class 2606 OID 16515)
-- Name: lessons_places lessons_places_fk_lesson; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons_places
    ADD CONSTRAINT lessons_places_fk_lesson FOREIGN KEY (lesson) REFERENCES lessons(id);


--
-- TOC entry 2754 (class 2606 OID 16520)
-- Name: students students_fk_class; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY students
    ADD CONSTRAINT students_fk_class FOREIGN KEY (class) REFERENCES classes(id);


-- Completed on 2018-02-06 20:20:14

--
-- PostgreSQL database dump complete
--
