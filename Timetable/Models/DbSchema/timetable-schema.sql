--
-- PostgreSQL database dump
--

-- Dumped from database version 9.5.1
-- Dumped by pg_dump version 9.5.1

-- Started on 2016-03-15 20:32:58

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 1 (class 3079 OID 12355)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2199 (class 0 OID 0)
-- Dependencies: 1
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 189 (class 1259 OID 24823)
-- Name: classes; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE classes (
    id integer NOT NULL,
    year integer NOT NULL,
    code_name text,
    tutor character varying(11) NOT NULL
);


ALTER TABLE classes OWNER TO postgres;

--
-- TOC entry 188 (class 1259 OID 24821)
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
-- TOC entry 2200 (class 0 OID 0)
-- Dependencies: 188
-- Name: classes_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE classes_id_seq OWNED BY classes.id;


--
-- TOC entry 183 (class 1259 OID 16699)
-- Name: classrooms; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE classrooms (
    id integer NOT NULL,
    name text NOT NULL,
    administrator character varying(11) NOT NULL
);


ALTER TABLE classrooms OWNER TO postgres;

--
-- TOC entry 182 (class 1259 OID 16697)
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
-- TOC entry 2201 (class 0 OID 0)
-- Dependencies: 182
-- Name: classrooms_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE classrooms_id_seq OWNED BY classrooms.id;


--
-- TOC entry 185 (class 1259 OID 16721)
-- Name: days; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE days (
    id integer NOT NULL,
    name text NOT NULL
);


ALTER TABLE days OWNER TO postgres;

--
-- TOC entry 184 (class 1259 OID 16719)
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
-- TOC entry 2202 (class 0 OID 0)
-- Dependencies: 184
-- Name: days_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE days_id_seq OWNED BY days.id;


--
-- TOC entry 187 (class 1259 OID 16762)
-- Name: hours; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE hours (
    id integer NOT NULL,
    hour time without time zone NOT NULL
);


ALTER TABLE hours OWNER TO postgres;

--
-- TOC entry 186 (class 1259 OID 16760)
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
-- TOC entry 2203 (class 0 OID 0)
-- Dependencies: 186
-- Name: hours_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE hours_id_seq OWNED BY hours.id;


--
-- TOC entry 194 (class 1259 OID 24971)
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
-- TOC entry 193 (class 1259 OID 24969)
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
-- TOC entry 2204 (class 0 OID 0)
-- Dependencies: 193
-- Name: lessons_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE lessons_id_seq OWNED BY lessons.id;


--
-- TOC entry 195 (class 1259 OID 25015)
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
-- TOC entry 190 (class 1259 OID 24853)
-- Name: students; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE students (
    pesel character varying(11) NOT NULL,
    first_name text NOT NULL,
    last_name text NOT NULL,
    class integer NOT NULL
);


ALTER TABLE students OWNER TO postgres;

--
-- TOC entry 192 (class 1259 OID 24906)
-- Name: subjects; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE subjects (
    id integer NOT NULL,
    name text NOT NULL
);


ALTER TABLE subjects OWNER TO postgres;

--
-- TOC entry 191 (class 1259 OID 24904)
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
-- TOC entry 2205 (class 0 OID 0)
-- Dependencies: 191
-- Name: subjects_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE subjects_id_seq OWNED BY subjects.id;


--
-- TOC entry 181 (class 1259 OID 16680)
-- Name: teachers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE teachers (
    pesel character varying(11) NOT NULL,
    first_name text NOT NULL,
    last_name text NOT NULL
);


ALTER TABLE teachers OWNER TO postgres;

--
-- TOC entry 2032 (class 2604 OID 24826)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY classes ALTER COLUMN id SET DEFAULT nextval('classes_id_seq'::regclass);


--
-- TOC entry 2029 (class 2604 OID 16702)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY classrooms ALTER COLUMN id SET DEFAULT nextval('classrooms_id_seq'::regclass);


--
-- TOC entry 2030 (class 2604 OID 16724)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY days ALTER COLUMN id SET DEFAULT nextval('days_id_seq'::regclass);


--
-- TOC entry 2031 (class 2604 OID 16765)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY hours ALTER COLUMN id SET DEFAULT nextval('hours_id_seq'::regclass);


--
-- TOC entry 2034 (class 2604 OID 24974)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons ALTER COLUMN id SET DEFAULT nextval('lessons_id_seq'::regclass);


--
-- TOC entry 2033 (class 2604 OID 24909)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY subjects ALTER COLUMN id SET DEFAULT nextval('subjects_id_seq'::regclass);


--
-- TOC entry 2185 (class 0 OID 24823)
-- Dependencies: 189
-- Data for Name: classes; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY classes (id, year, code_name, tutor) FROM stdin;
\.


--
-- TOC entry 2206 (class 0 OID 0)
-- Dependencies: 188
-- Name: classes_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('classes_id_seq', 1, false);


--
-- TOC entry 2179 (class 0 OID 16699)
-- Dependencies: 183
-- Data for Name: classrooms; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY classrooms (id, name, administrator) FROM stdin;
\.


--
-- TOC entry 2207 (class 0 OID 0)
-- Dependencies: 182
-- Name: classrooms_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('classrooms_id_seq', 1, false);


--
-- TOC entry 2181 (class 0 OID 16721)
-- Dependencies: 185
-- Data for Name: days; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY days (id, name) FROM stdin;
\.


--
-- TOC entry 2208 (class 0 OID 0)
-- Dependencies: 184
-- Name: days_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('days_id_seq', 1, false);


--
-- TOC entry 2183 (class 0 OID 16762)
-- Dependencies: 187
-- Data for Name: hours; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY hours (id, hour) FROM stdin;
\.


--
-- TOC entry 2209 (class 0 OID 0)
-- Dependencies: 186
-- Name: hours_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('hours_id_seq', 1, false);


--
-- TOC entry 2190 (class 0 OID 24971)
-- Dependencies: 194
-- Data for Name: lessons; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY lessons (id, teacher, subject, class) FROM stdin;
\.


--
-- TOC entry 2210 (class 0 OID 0)
-- Dependencies: 193
-- Name: lessons_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('lessons_id_seq', 1, false);


--
-- TOC entry 2191 (class 0 OID 25015)
-- Dependencies: 195
-- Data for Name: lessons_places; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY lessons_places (lesson, classroom, day, hour) FROM stdin;
\.


--
-- TOC entry 2186 (class 0 OID 24853)
-- Dependencies: 190
-- Data for Name: students; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY students (pesel, first_name, last_name, class) FROM stdin;
\.


--
-- TOC entry 2188 (class 0 OID 24906)
-- Dependencies: 192
-- Data for Name: subjects; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY subjects (id, name) FROM stdin;
\.


--
-- TOC entry 2211 (class 0 OID 0)
-- Dependencies: 191
-- Name: subjects_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('subjects_id_seq', 1, false);


--
-- TOC entry 2177 (class 0 OID 16680)
-- Dependencies: 181
-- Data for Name: teachers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY teachers (pesel, first_name, last_name) FROM stdin;
\.


--
-- TOC entry 2044 (class 2606 OID 24831)
-- Name: classes_pk_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY classes
    ADD CONSTRAINT classes_pk_id PRIMARY KEY (id);


--
-- TOC entry 2038 (class 2606 OID 16707)
-- Name: classrooms_pk_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY classrooms
    ADD CONSTRAINT classrooms_pk_id PRIMARY KEY (id);


--
-- TOC entry 2040 (class 2606 OID 16729)
-- Name: days_pk_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY days
    ADD CONSTRAINT days_pk_id PRIMARY KEY (id);


--
-- TOC entry 2042 (class 2606 OID 16767)
-- Name: hours_pk_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY hours
    ADD CONSTRAINT hours_pk_id PRIMARY KEY (id);


--
-- TOC entry 2050 (class 2606 OID 24976)
-- Name: lessons_pk_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons
    ADD CONSTRAINT lessons_pk_id PRIMARY KEY (id);


--
-- TOC entry 2052 (class 2606 OID 25019)
-- Name: lessons_places_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons_places
    ADD CONSTRAINT lessons_places_pk PRIMARY KEY (lesson, classroom, day, hour);


--
-- TOC entry 2046 (class 2606 OID 24860)
-- Name: students_pk_pesel; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY students
    ADD CONSTRAINT students_pk_pesel PRIMARY KEY (pesel);


--
-- TOC entry 2048 (class 2606 OID 24914)
-- Name: subjects_pk_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY subjects
    ADD CONSTRAINT subjects_pk_id PRIMARY KEY (id);


--
-- TOC entry 2036 (class 2606 OID 16687)
-- Name: teachers_pk_pesel; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY teachers
    ADD CONSTRAINT teachers_pk_pesel PRIMARY KEY (pesel);


--
-- TOC entry 2054 (class 2606 OID 24832)
-- Name: classes_fk_tutor; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY classes
    ADD CONSTRAINT classes_fk_tutor FOREIGN KEY (tutor) REFERENCES teachers(pesel);


--
-- TOC entry 2053 (class 2606 OID 24884)
-- Name: classrooms_fk_administrator; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY classrooms
    ADD CONSTRAINT classrooms_fk_administrator FOREIGN KEY (administrator) REFERENCES teachers(pesel);


--
-- TOC entry 2058 (class 2606 OID 24987)
-- Name: lessons_fk_class; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons
    ADD CONSTRAINT lessons_fk_class FOREIGN KEY (class) REFERENCES classes(id);


--
-- TOC entry 2057 (class 2606 OID 24982)
-- Name: lessons_fk_subject; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons
    ADD CONSTRAINT lessons_fk_subject FOREIGN KEY (subject) REFERENCES subjects(id);


--
-- TOC entry 2056 (class 2606 OID 24977)
-- Name: lessons_fk_teacher; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons
    ADD CONSTRAINT lessons_fk_teacher FOREIGN KEY (teacher) REFERENCES teachers(pesel);


--
-- TOC entry 2060 (class 2606 OID 25025)
-- Name: lessons_places_fk_classroom; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons_places
    ADD CONSTRAINT lessons_places_fk_classroom FOREIGN KEY (classroom) REFERENCES classrooms(id);


--
-- TOC entry 2061 (class 2606 OID 25030)
-- Name: lessons_places_fk_day; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons_places
    ADD CONSTRAINT lessons_places_fk_day FOREIGN KEY (day) REFERENCES days(id);


--
-- TOC entry 2062 (class 2606 OID 25035)
-- Name: lessons_places_fk_hour; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons_places
    ADD CONSTRAINT lessons_places_fk_hour FOREIGN KEY (hour) REFERENCES hours(id);


--
-- TOC entry 2059 (class 2606 OID 25020)
-- Name: lessons_places_fk_lesson; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lessons_places
    ADD CONSTRAINT lessons_places_fk_lesson FOREIGN KEY (lesson) REFERENCES lessons(id);


--
-- TOC entry 2055 (class 2606 OID 24861)
-- Name: students_fk_class; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY students
    ADD CONSTRAINT students_fk_class FOREIGN KEY (class) REFERENCES classes(id);


--
-- TOC entry 2198 (class 0 OID 0)
-- Dependencies: 6
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2016-03-15 20:32:59

--
-- PostgreSQL database dump complete
--

